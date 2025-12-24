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

        // Get: /Account/Edit
        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.UserId = userManager.GetUserId(HttpContext.User);
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return NotFound();

            var model = new UserViewModel()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleName = user.RoleName,
                CreatedOn = user.CreatedOn
            };

            return View(model);
        }

        // Post: /Account/Edit
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId.ToString());

            if (user is null)
                return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            await userManager.SetEmailAsync(user, model.Email);
            await userManager.SetUserNameAsync(user, model.Email);

            user.RoleName = model.RoleName;

            // Audit Columns
            user.CreatedOn = model.CreatedOn;
            user.ModifiedOn = DateTime.UtcNow;

            var result = await userManager.UpdateAsync(user);

            if(result.Succeeded)
                return RedirectToAction(nameof(Index));

            return View(model);
        }



        // Get: /Account/Details
        [HttpGet("Details"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return NotFound();

            var model = new UserViewModel()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleName = user.RoleName,
                CreatedOn = user.CreatedOn
            };

            return View(model);
        }



        // Get: /Account/Delete
        [HttpGet("Delete"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return NotFound();

            var model = new UserViewModel()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleName = user.RoleName,
                CreatedOn = user.CreatedOn
            };

            return View(model);
        }

        // Post: /Account/Delete
        [HttpPost("Delete"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(UserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId.ToString());

            if (user is null) 
                return NotFound();

            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            return View(model);
        }
    }
}
