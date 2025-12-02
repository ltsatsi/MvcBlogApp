using MyBlogApplication.Models;

namespace MyBlogApplication.Interfaces
{
    public interface ICommentRepo
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync(string sortOrder);
        Task<Comment> GetCommentByIdAsync(int id);
        Task<Comment> CreateCommentAsync(Comment comment);
        Task<Comment> UpdateCommentAsync(Comment comment);
        Task<Comment> DeleteCommentAsync(Comment commment); 
        Task<bool> IsExistAsync(int id);
    }
}
