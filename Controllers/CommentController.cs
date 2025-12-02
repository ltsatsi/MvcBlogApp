using Microsoft.AspNetCore.Mvc;
using MyBlogApplication.Infrastructure;
using MyBlogApplication.Interfaces;
using MyBlogApplication.Models;
using MyBlogApplication.Repositories;
using System.Reflection.Metadata;
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
        public async Task<IActionResult> Index(string sortOrder, int? pageNumber)
        {
            int pageSize = 6;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["AuthorNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "author_name_desc" : "";
            ViewData["CreatedAtSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";
            return View(PaginatedList<Comment>.Create(await _commentRepo.GetAllCommentsAsync(sortOrder), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.ShowButton = true;
            var model = await _commentRepo.GetCommentByIdAsync(id); 
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Comment comment)
        {
            ViewBag.ShowButton = false;
            ViewBag.SuccessMessage = $"Comment successfully deleted.";
            await _commentRepo.DeleteCommentAsync(comment);
            return View(comment);
        }
    }
}
