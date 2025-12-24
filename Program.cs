using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlogApplication.Data;
using MyBlogApplication.Interfaces;
using MyBlogApplication.Models;
using MyBlogApplication.Repositories;

namespace MyBlogApplication
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register Entity Framework ORM
            builder.Services.AddDbContext<AppDBContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new Exception("Connection string not found.")));

            // Register Dependency Injection
            builder.Services.AddScoped<IDBInitialiser, DBInitialisers>();
            builder.Services.AddScoped<IBlogRepo, BlogRepo>();
            builder.Services.AddScoped<ICommentRepo, CommentRepo>();
            builder.Services.AddScoped<IImageUpload, ImageUpload>();

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<AppDBContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

                app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = 
                    scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                var roles = new[]
                {
                    "Admin",
                    "Editor",
                    "User"
                };

                foreach ( var role in roles )
                {
                    var appRole = new ApplicationRole(role);

                    switch (role)
                    {
                        case "User":
                            appRole.Description = "Standard user, Inidvidual application access.";
                            appRole.CreatedOn = DateTime.UtcNow;
                            appRole.ModifiedOn = DateTime.UtcNow;
                            break;
                        case "Admin":
                            appRole.Description = "Administrator, Administration access across whole application.";
                            appRole.CreatedOn = DateTime.UtcNow;
                            appRole.ModifiedOn = DateTime.UtcNow;
                            break;
                        case "Editor":
                            appRole.Description = "Editor, Has writing and publishing access.";
                            appRole.CreatedOn = DateTime.UtcNow;
                            appRole.ModifiedOn = DateTime.UtcNow;
                            break;
                        default:
                            break;
                    }

                    await roleManager.CreateAsync(appRole);
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = 
                    scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string email = "admin1@gmail.com";
                string password = "AdminPassword1#";

                if(await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser()
                    {
                        FirstName = "Administrator",
                        LastName = "01",
                        UserName = email,
                        Email = email,
                        RoleName = "Admin",
                        LockoutEnabled = true,

                        CreatedOn = DateTime.UtcNow,
                        ModifiedOn = DateTime.UtcNow,
                    };

                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }
            
            await app.RunAsync();
        }
    }
}
