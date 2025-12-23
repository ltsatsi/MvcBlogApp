using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlogApplication.Infrastructure;
using MyBlogApplication.Interfaces;
using MyBlogApplication.Models;
using MyBlogApplication.ViewModel;
using System.Threading.Tasks;

namespace MyBlogApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IBlogRepo _blogRepo;
        private readonly ICommentRepo _commentRepo;
        private readonly UserManager<ApplicationUser> _userManager; 
        public AdminController(
            IBlogRepo blogRepo, 
            ICommentRepo commentRepo,
            UserManager<ApplicationUser> userManager)
        {
            _blogRepo = blogRepo;
            _commentRepo = commentRepo;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel(_userManager)
            {
                Blogs = PaginatedList<Blog>.Create(await _blogRepo.GetAllBlogsAsync(sortOrder: "date_desc"),  1, 2),
                Comments = PaginatedList<Comment>.Create(await _commentRepo.GetAllCommentsAsync(sortOrder: "date_desc"), 1, 3),
            };

            return View("Dashboard", model);
        }
    }
}
