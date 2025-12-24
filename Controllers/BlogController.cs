using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MyBlogApplication.Infrastructure;
using MyBlogApplication.Interfaces;
using MyBlogApplication.Models;
using MyBlogApplication.Repositories;

namespace MyBlogApplication.Controllers
{
    [Route("blog"), Authorize]
    public class BlogController : Controller
    {
        private readonly ICommentRepo _commentRepo;
        private readonly IBlogRepo _blogRepo;
        private readonly IImageUpload _imageUpload;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogController(
            IBlogRepo blogRepo, 
            ICommentRepo commentRepo, 
            IImageUpload imageUpload,
            UserManager<ApplicationUser> userManager)
        {
            _blogRepo = blogRepo;
            _commentRepo = commentRepo;
            _imageUpload = imageUpload;
            _userManager = userManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string searchInput, string sortOrder, string currentFilter, int? pageNumber)
        {
            pageNumber = pageNumber ?? 1;
            int pageSize = 4;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CategorySortParam"] = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "";
            ViewData["TitleSortParam"]= String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchInput != null)
            {
                pageNumber = 1;
            } 
            else 
            {
                searchInput = currentFilter;
            }

            ViewData["CurrentFilter"] = searchInput;


            ViewData["CurrentSearch"] = searchInput;
            return View(PaginatedList<Blog>.Create(await _blogRepo.GetAllBlogsAsync(searchInput, sortOrder), pageNumber ?? 1, pageSize));
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewBag.UserId = Guid.Parse(_userManager.GetUserId(HttpContext.User) ?? 
                throw new Exception("No user found"));
            ViewBag.ShowButton = true;
            return View(new Blog());
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create(Blog blog, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ShowButton = false;
                blog.ImageUrl = await _imageUpload.UploadImageAsync(imageFile, "images/posts");
                ViewBag.SuccessMessage = $"Article successfully published.";
                await _blogRepo.CreateBlogAsync(blog);

                return View(blog);
            }
            
            return View(blog);
        }

        [HttpGet("details-default/{id}")]
        public async Task<IActionResult> DetailsDefault(int id)
        {
            ViewBag.UserId = Guid.Parse(_userManager.GetUserId(HttpContext.User) ??
                throw new Exception("No user found"));
            var model = await _blogRepo.GetBlogByIdAsync(id);
            return View("Details", model);
        }

        [HttpGet("details-query")]
        public async Task<IActionResult> DetailsQuery([FromQuery(Name = "id")] int id)
        {
            ViewBag.UserId = Guid.Parse(_userManager.GetUserId(HttpContext.User) ??
                throw new Exception("No user found"));
            var model = await _blogRepo.GetBlogByIdAsync(id);
            return View("Details", model);
        }

        [HttpPost("details-form")]
        public async Task<IActionResult> DetailsForm([FromForm] int id)
        {
            var model = await _blogRepo.GetBlogByIdAsync(id);
            return RedirectToAction(nameof(DetailsDefault), new { id });
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.UserId = Guid.Parse(_userManager.GetUserId(HttpContext.User) ??
                throw new Exception("No user found"));
            ViewBag.ShowButton = true;
            var model = await _blogRepo.GetBlogByIdAsync(id);
            return View(model);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(Blog blog, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ShowButton = false;
                if (imageFile != null && imageFile.Length > 0)
                {
                    blog.ImageUrl = await _imageUpload.UploadImageAsync(imageFile, "images/posts");
                }
                ViewBag.SuccessMessage = $"Article successfully updated.";
                await _blogRepo.UpdateBlogAsync(blog);

                return View(blog);
            }

            return View(blog);
        }

        [HttpGet("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.ShowButton = true;
            var model = await _blogRepo.GetBlogByIdAsync(id);
            return View(model);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(Blog blog)
        {
            ViewBag.ShowButton = false;
            ViewBag.SuccessMessage = $"Blog post successfully deleted.";
            await _blogRepo.DeleteBlogAsync(blog);
            return View(blog);
        }


        [HttpPost]  
        public async Task<IActionResult> AddComment(int blogId, string authorId, string content)
        {
            if (ModelState.IsValid)
            {
                var blog = await _blogRepo.GetBlogByIdAsync(blogId);

                if (blog == null)
                    return NotFound("Blog not found");

                Comment commnent = new Comment()
                {
                    BlogId = blogId,
                    AuthorId = Guid.Parse(authorId),
                    Content = content
                };

                await _commentRepo.CreateCommentAsync(commnent);
                return RedirectToAction(nameof(DetailsDefault), new { id = blogId });
            }

            return RedirectToAction(nameof(DetailsDefault), new { id = blogId });
        }
    }
}
