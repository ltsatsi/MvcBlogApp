using MyBlogApplication.Models;

namespace MyBlogApplication.Interfaces
{
    public interface IBlogRepo
    {
        Task<IEnumerable<Blog>> GetAllBlogsAsync(string searchInput = "", string sortOrder = "");    
        Task<Blog> GetBlogByIdAsync(int id);
        Task<Blog> CreateBlogAsync(Blog blog);
        Task<Blog> UpdateBlogAsync(Blog blog);
        Task<Blog> DeleteBlogAsync(Blog blog);
        Task<bool> IsExistAsync(int id);
    }
}
