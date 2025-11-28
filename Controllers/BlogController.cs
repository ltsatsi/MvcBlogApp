using Microsoft.AspNetCore.Mvc;
using MyBlogApplication.Infrastructure;
using MyBlogApplication.Interfaces;
using MyBlogApplication.Models;
using MyBlogApplication.Repositories;

namespace MyBlogApplication.Controllers
{
    public class BlogController : Controller
    {
        private readonly ICommentRepo _commentRepo;
        private readonly IBlogRepo _blogRepo;
        public BlogController(IBlogRepo blogRepo, ICommentRepo commentRepo)
        {
            _blogRepo = blogRepo;
            _commentRepo = commentRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchInput, string sortOrder, string currentFilter, int? pageNumber)
        {
            pageNumber = pageNumber ?? 1;
            int pageSize = 3;
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new Blog());
        }


        [HttpPost]
        public async Task<IActionResult> Create(Blog blog, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // upload images
                var folder = Path.Combine("wwwroot", "images", "posts");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // file processing
                var fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                var filePath = Path.Combine(folder, fileName);

                // file processing
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Set image url
                blog.ImageUrl = $"/images/posts/{fileName}";

                ViewBag.SuccessMessage = $"Blog post \'{blog.Title}\' successfully created.";
                await _blogRepo.CreateBlogAsync(blog);

                return View(blog);
            }

            return View(blog);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _blogRepo.GetBlogByIdAsync(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _blogRepo.GetBlogByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Blog blog, IFormFile imageFile)
        {
            // upload images
            var folder = Path.Combine("wwwroot", "images", "posts");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // file processing
            var fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
            var filePath = Path.Combine(folder, fileName);

            // file processing
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Set image url
            blog.ImageUrl = $"/images/posts/{fileName}";

            ViewBag.SuccessMessage = $"Blog post \'{blog.Title}\' successfully updated.";
            await _blogRepo.UpdateBlogAsync(blog);
            return View(blog);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _blogRepo.GetBlogByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Blog blog)
        {
            ViewBag.SuccessMessage = $"Blog post \'{blog.Title}\' successfully deleted.";
            await _blogRepo.DeleteBlogAsync(blog);
            return View(blog);
        }


        [HttpPost]  
        public async Task<IActionResult> AddComment(int blogId, string authorName, string content)
        {
            var blog = await _blogRepo.GetBlogByIdAsync(blogId);

            if (blog == null)
                return NotFound("Blog not found");

            Comment commnent = new Comment()
            {
                    BlogId = blogId,
                    AuthorName = authorName,
                    Content = content
            };

                await _commentRepo.CreateCommentAsync(commnent);
                return RedirectToAction("Details", new { id = blogId });
        }
    }
}
