using Microsoft.AspNetCore.Identity;
using MyBlogApplication.Infrastructure;
using MyBlogApplication.Models;

namespace MyBlogApplication.ViewModel
{
    public class AdminDashboardViewModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminDashboardViewModel()
        {
            
        }
        public AdminDashboardViewModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public PaginatedList<Blog> Blogs { get; set; }
        public PaginatedList<Comment> Comments { get; set; }
        public int UserCount => _userManager.Users.Count();
        public int BlogCount => Blogs?.TotalCount ?? 0;
        public int CommentCount => Comments?.TotalCount ?? 0;
    }
}
