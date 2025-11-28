using Microsoft.AspNetCore.Mvc;
using MyBlogApplication.Interfaces;
using MyBlogApplication.Models;
using MyBlogApplication.Repositories;
using System.Threading.Tasks;

namespace MyBlogApplication.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepo _commentRepo;
        public CommentController(ICommentRepo commentRepo)
        {

            _commentRepo = commentRepo;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _commentRepo.GetAllCommentsAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _commentRepo.GetCommentByIdAsync(id); 
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Comment comment)
        {
            await _commentRepo.DeleteCommentAsync(comment);
            return View(comment);
        }
    }
}
