using Microsoft.EntityFrameworkCore;
using MyBlogApplication.Data;
using MyBlogApplication.Interfaces;
using MyBlogApplication.Models;

namespace MyBlogApplication.Repositories
{
    public class BlogRepo : IBlogRepo
    {
        private readonly AppDBContext _context;
        public BlogRepo(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Blog> CreateBlogAsync(Blog blog)
        {
            await _context.Blogs.AddAsync(blog);   
            _context.SaveChanges();

            return blog;
        }

        public async Task<Blog> DeleteBlogAsync(Blog blog)
        {
            _context.Blogs.Remove(blog);
            _context.SaveChanges();

            return blog;
        }

        public async Task<IEnumerable<Blog>> GetAllBlogsAsync(string searchInput)
        {
            var blogs = await _context.Blogs.ToListAsync();
            if (!string.IsNullOrEmpty(searchInput))
            {
                blogs = blogs.Where(b => b.Title.ToLower().Contains(searchInput.ToLower())).ToList();
            }
            return blogs;
        }

        public async Task<Blog> GetBlogByIdAsync(int id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(p => p.BlogId == id);
            return blog;
        }

        public async Task<bool> IsExistAsync(int id)
        {
            bool isExist = false;
            var blog = await _context.Blogs.FirstOrDefaultAsync(p => p.BlogId == id);

            if (blog != null)
            {
                isExist = true;
            }

            return isExist;
        }

        public async Task<Blog> UpdateBlogAsync(Blog blog)
        {
            _context.Blogs.Update(blog);
            _context.SaveChanges();

            return blog;
        }
    }
}
