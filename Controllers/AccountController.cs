using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlogApplication.Models;
using MyBlogApplication.ViewModel;
using System.Threading.Tasks;

namespace MyBlogApplication.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users
                .Select(u => new UserViewModel()
                {
                    UserId = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName ?? null,
                    Email = u.Email,
                    RoleName = u.RoleName,
                    CreatedOn = u.CreatedOn
                })
                .ToListAsync();

            return View(users);
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, model.Password);

            // Assign default role
            await userManager.AddToRoleAsync(user, "User");
            user.RoleName = "User";

            if (result.Succeeded)
                return RedirectToAction(nameof(Index), "Home");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return NotFound($"user -- {model.Email} Not found.");

            var result = await signInManager.PasswordSignInAsync(
                user, 
                model.Password, 
                isPersistent: true, 
                lockoutOnFailure: false
            );

            if (result.Succeeded)
                return RedirectToAction(nameof(Index), "Home");

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            return View(model);
        }

        [HttpGet("Signout")]
        public new async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
