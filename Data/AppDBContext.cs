using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBlogApplication.Models;

namespace MyBlogApplication.Data
{
    public class AppDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) 
            : base(options)
        {

        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
