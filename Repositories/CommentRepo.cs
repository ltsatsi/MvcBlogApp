using Microsoft.EntityFrameworkCore;
using MyBlogApplication.Data;
using MyBlogApplication.Interfaces;
using MyBlogApplication.Models;

namespace MyBlogApplication.Repositories
{
    public class CommentRepo : ICommentRepo
    {
        private readonly AppDBContext _context;
        public CommentRepo(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment> DeleteCommentAsync(Comment commment)
        {
            _context.Comments.Remove(commment);
            await _context.SaveChangesAsync();

            return commment;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            var comments = await _context.Comments.ToListAsync();
            return comments;
        }

        public async Task<Comment> GetCommentByIdAsync(int id)  
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == id);
            return comment;
        }

        public async Task<bool> IsExistAsync(int id)
        {
            bool isExist = false;
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == id);

            if (comment != null)
            {
                isExist = true;
            }

            return isExist;
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}
