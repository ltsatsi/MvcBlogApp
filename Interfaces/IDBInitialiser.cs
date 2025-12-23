using Microsoft.AspNetCore.Identity;
using MyBlogApplication.Data;
using MyBlogApplication.Models;

namespace MyBlogApplication.Interfaces
{
    public interface IDBInitialiser
    {
        Task InitialiseAsync(AppDBContext context, UserManager<ApplicationUser> userManager);
    }
}
