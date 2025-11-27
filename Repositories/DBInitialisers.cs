using MyBlogApplication.Data;
using MyBlogApplication.Interfaces;
using MyBlogApplication.Models;

namespace MyBlogApplication.Repositories
{
    public class DBInitialisers : IDBInitialiser
    {
        public void Initialise(AppDBContext context)
        {
            context.Database.EnsureCreated();

            if (context.Blogs.Any())
            {
                return;
            }

            var blogs = new Blog[]
            {
                new Blog() 
                {
                    BlogId = 1,
                    Author = "James Perkins",
                    Title = "The Rise of AI-Driven Development",
                    ImageUrl = "/images/posts/hands.jpg",
                    Category = "Technology",
                    Content = "Artificial intelligence is reshaping how developers build software. From AI-assisted coding tools to automated testing and deployment, modern workflows are becoming faster and more efficient. In this blog, we explore how AI is impacting software development today and what it means for future engineers.",
                    Summary = "How AI tools are transforming software development workflows and changing the future of engineering."
                },

                new Blog() 
                {
                    BlogId = 2,
                    Author = "Lexis Haun",
                    Title = "5 Cloud Technologies Dominating 2025",
                    ImageUrl = "/images/posts/cloud.jpg",
                    Category = "Technology",
                    Content = "Cloud computing continues to evolve with new services that improve scalability, resilience, and cost-efficiency. This article highlights the top five cloud technologies in 2025, including serverless computing, edge processing, zero-trust security models, AI-enhanced cloud services, and multi-cloud orchestration.",
                    Summary = "A look at the top cloud innovations shaping the tech industry in 2025."
                },

                new Blog() 
                {
                    BlogId = 3,
                    Author = "Paul Rogan",
                    Title = "Smart Farming: The Future of Agriculture",
                    ImageUrl = "/images/posts/farm.jpg",
                    Category = "Agriculture",
                    Content = "Smart farming integrates sensors, IoT devices, and data analytics to improve crop yield and reduce waste. Farmers can now monitor soil health, automate irrigation, and predict harvest outcomes with precision. This blog explains how digital tools are transforming modern agriculture.",
                    Summary = "How IoT and data analytics are revolutionizing farming efficiency."
                },

                new Blog() 
                {
                    BlogId = 4,
                    Author = "Thabo Maikela",
                    Title = "Sustainable Agriculture in a Changing Climate",
                    ImageUrl = "/images/posts/climate.jpg",
                    Category = "Agriculture",
                    Content = "Climate change is pushing farmers to adopt new approaches that conserve resources and improve resilience. Techniques such as crop rotation, regenerative soil practices, and water-efficient irrigation are becoming essential. This article explores sustainable methods used in agriculture today.",
                    Summary = "Exploring modern sustainable farming practices in response to climate change."
                },

                new Blog() 
                {
                    BlogId = 5,
                    Author = "Anderson Peit",
                    Title = "Innovation Through Design Thinking",
                    Category = "Innovation",
                    Content = "Design thinking encourages problem-solving through empathy, experimentation, and iterative design. Businesses worldwide use this approach to fuel innovation, create user-centered products, and stay competitive. This blog breaks down the core stages of design thinking and its real-world impact.",
                    Summary = "How design thinking helps organizations innovate and build user-focused solutions."
                }

            };

            foreach (var b in blogs)
            {
                context.Blogs.Add(b);
            }

            context.SaveChanges();
        }
    }
}
