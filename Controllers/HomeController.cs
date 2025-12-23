using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlogApplication.Data;
using MyBlogApplication.Interfaces;
using MyBlogApplication.Models;
using MyBlogApplication.ViewModel;
using System.Diagnostics;

namespace MyBlogApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDBContext _context;
        private readonly IDBInitialiser _seedDb;

        public HomeController(
            ILogger<HomeController> logger, 
            AppDBContext context, 
            IDBInitialiser seedDb, 
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _seedDb = seedDb;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SeedDatabase()
        {
            await _seedDb.InitialiseAsync(_context, _userManager);
            ViewBag.SeedFeedback = "Database created.";
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            if (userId is null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return NotFound();

            var model = new UserViewModel()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName ?? null,
                Email = user.Email,
                RoleName = user.RoleName,
                CreatedOn = user.CreatedOn
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
