using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyBlogApplication.Data;
using MyBlogApplication.Interfaces;
using MyBlogApplication.Models;

namespace MyBlogApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _context;
        private readonly IDBInitialiser _seedDb;

        public HomeController(ILogger<HomeController> logger, AppDBContext context, IDBInitialiser seedDb)
        {
            _logger = logger;
            _context = context;
            _seedDb = seedDb;
        }

        public IActionResult SeedDatabase()
        {
            _seedDb.Initialise(_context);
            ViewBag.SeedFeedback = "Database created.";
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
