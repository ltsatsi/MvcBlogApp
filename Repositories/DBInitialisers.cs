using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using MyBlogApplication.Data;
using MyBlogApplication.Interfaces;
using MyBlogApplication.Models;

namespace MyBlogApplication.Repositories
{
    public class DBInitialisers : IDBInitialiser
    {
        public async Task InitialiseAsync(
            AppDBContext context, 
            UserManager<ApplicationUser> userManager)
        {
            context.Database.EnsureCreated();

            // Seed users
            if (userManager.Users.Any())
            {
                return;
            }

            var users = new ApplicationUser[]
            {
                new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "James",
                    LastName = "Perkins",
                    RoleName = "User",
                    UserName = "jamesperkins@gmail.com",
                    Email = "jamesperkins@gmail.com",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },


                new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Lexis",
                    LastName = "Haun",
                    RoleName = "User",
                    UserName = "lexishuan@gmail.com",
                    Email = "lexishuan@gmail.com",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },


                new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Paul",
                    LastName = "Rogan",
                    RoleName = "User",
                    UserName = "paulrogan@gmail.com",
                    Email = "paulrogan@gmail.com",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },


                new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Thabo",
                    LastName = "Maikela",
                    RoleName = "User",
                    UserName = "thabomaikela@gmail.com",
                    Email = "thabomaikela@gmail.com",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },


                new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Anderson",
                    LastName = "Piet",
                    RoleName = "User",
                    UserName= "andersonpiet@gmail.com",
                    Email = "andersonpiet@gmail.com",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
            };

            foreach(var user in users)
            {
                if(await userManager.FindByIdAsync(user.Id.ToString()) == null)
                {
                    await userManager.CreateAsync(user, "Password1#");
                }
            }

            var james = await userManager.FindByEmailAsync("jamesperkins@gmail.com");
            var lexis = await userManager.FindByEmailAsync("lexishuan@gmail.com");
            var paul = await userManager.FindByEmailAsync("paulrogan@gmail.com");
            var thabo = await userManager.FindByEmailAsync("thabomaikela@gmail.com");
            var anderson = await userManager.FindByEmailAsync("andersonpiet@gmail.com");

            // Seed Blogs
            if (context.Blogs.Any())
            {
                return;
            }

            var blogs = new Blog[]
            {
                new Blog()
                {
                    BlogId = 1,
                    Title = "The Rise of AI-Driven Development",
                    ImageUrl = "/images/posts/hands.jpg",
                    Category = "Technology",
                    Content = "Artificial intelligence is reshaping how developers build software. From AI-assisted coding tools to automated testing and deployment, modern workflows are becoming faster and more efficient. In this blog, we explore how AI is impacting software development today and what it means for future engineers.",
                    Summary = "How AI tools are transforming software development workflows and changing the future of engineering.",
                    AuthorId = james.Id,
                    Comments = new List<Comment>()
                    {
                        new Comment() { CommentId = 1, Content = "Good Stuff ✨", BlogId= 1, AuthorId = james.Id },
                        new Comment() { CommentId = 2, Content = "AI is fantastic", BlogId= 1, AuthorId = james.Id },
                    }
                },

                new Blog()
                {
                    BlogId = 2,
                    Title = "5 Cloud Technologies Dominating 2025",
                    ImageUrl = "/images/posts/cloud.jpg",
                    Category = "Technology",
                    Content = "Cloud computing continues to evolve with new services that improve scalability, resilience, and cost-efficiency. This article highlights the top five cloud technologies in 2025, including serverless computing, edge processing, zero-trust security models, AI-enhanced cloud services, and multi-cloud orchestration.",
                    Summary = "A look at the top cloud innovations shaping the tech industry in 2025.",
                    AuthorId = lexis.Id,
                    Comments = new List<Comment>()
                    {
                        new Comment() { CommentId = 3, Content = "Love your work ❤️", BlogId= 2, AuthorId = lexis.Id },
                        new Comment() { CommentId = 4, Content = "Cloud is dominating", BlogId= 2, AuthorId = lexis.Id },
                    }
                },

                new Blog()
                {
                    BlogId = 3,
                    Title = "Smart Farming: The Future of Agriculture",
                    ImageUrl = "/images/posts/farm.jpg",
                    Category = "Agriculture",
                    Content = "Smart farming integrates sensors, IoT devices, and data analytics to improve crop yield and reduce waste. Farmers can now monitor soil health, automate irrigation, and predict harvest outcomes with precision. This blog explains how digital tools are transforming modern agriculture.",
                    Summary = "How IoT and data analytics are revolutionizing farming efficiency.",
                    AuthorId = paul.Id,
                    Comments = new List<Comment>()
                    {
                        new Comment() { CommentId = 5, Content = "Nice Stuff 🤗", BlogId=3, AuthorId = paul.Id },
                        new Comment() { CommentId = 6, Content = "My father has a farm", BlogId= 3, AuthorId = paul.Id },
                    }
                },

                new Blog()
                {
                    BlogId = 4,
                    Title = "Sustainable Agriculture in a Changing Climate",
                    ImageUrl = "/images/posts/climate.jpg",
                    Category = "Agriculture",
                    Content = "Climate change is pushing farmers to adopt new approaches that conserve resources and improve resilience. Techniques such as crop rotation, regenerative soil practices, and water-efficient irrigation are becoming essential. This article explores sustainable methods used in agriculture today.",
                    Summary = "Exploring modern sustainable farming practices in response to climate change.",
                    AuthorId = thabo.Id,
                    Comments = new List<Comment>()
                    {
                        new Comment() { CommentId = 7, Content = "What a nice blog", BlogId= 4, AuthorId = thabo.Id },
                        new Comment() { CommentId = 8, Content = "Gloabal warming is the source 💭", BlogId= 4, AuthorId = thabo.Id },
                    }
                },

                new Blog()
                {
                    BlogId = 5,
                    Title = "Innovation Through Design Thinking",
                    ImageUrl = "/images/posts/design.jpg",
                    Category = "Innovation",
                    Content = "Design thinking encourages problem-solving through empathy, experimentation, and iterative design. Businesses worldwide use this approach to fuel innovation, create user-centered products, and stay competitive. This blog breaks down the core stages of design thinking and its real-world impact.",
                    Summary = "How design thinking helps organizations innovate and build user-focused solutions.",
                    AuthorId = anderson.Id,
                    Comments = new List<Comment>()
                    {
                        new Comment() { CommentId = 9, Content = "Great something worth reading 🤓", BlogId= 5, AuthorId = anderson.Id },
                        new Comment() { CommentId = 10, Content = "Productivity Leads to innovation", BlogId= 5, AuthorId = anderson.Id },
                    }
                }

            };

            foreach (var b in blogs)
            {
                await context.Blogs.AddAsync(b);
            }

            await context.SaveChangesAsync();
        }
    }
}
