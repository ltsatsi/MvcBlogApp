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
            await _context.SaveChangesAsync();

            return blog;
        }

        public async Task<Blog> DeleteBlogAsync(Blog blog)
        {
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return blog;
        }

        public async Task<IEnumerable<Blog>> GetAllBlogsAsync(string searchInput = "", string sortOrder = "")
        {
            var blogs = await _context.Blogs
                .Include(b => b.Comments)
                .ThenInclude(b => b.Author)
                .ToListAsync();
            if (!string.IsNullOrEmpty(searchInput))
            {
                blogs = blogs.Where(b => b.Title.ToLower().Contains(searchInput.ToLower())).ToList();
            }
            switch (sortOrder)
            {
                case "category_desc":
                    blogs = blogs.OrderByDescending(b => b.Category).ToList();
                    break;
                case "title_desc":
                    blogs = blogs.OrderByDescending(b => b.Title).ToList();
                    break;
                case "date_desc":
                    blogs = blogs.OrderByDescending(b => b.CreatedAt).ToList();
                    break;
                default:
                    blogs = blogs.OrderBy(b => b.CreatedAt).ToList();
                    break;
            }
            return blogs;
        }

        public async Task<Blog> GetBlogByIdAsync(int id)
        {
            var blog = await _context.Blogs
                .Include(b => b.Comments)
                .Include(b => b.Author)
                .FirstOrDefaultAsync(p => p.BlogId == id);

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
            await _context.SaveChangesAsync();

            return blog;
        }
    }
}
