using Microsoft.AspNetCore.Mvc;
using Proj.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proj.Data;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Proj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ApplicationDbContext context)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index() => View();

        public IActionResult Main()
        {
            var cameras = _context.Cameras.Where(c => c.IsAvailable).ToList();
            return View(cameras);
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            ViewBag.UserName = user.FullName ?? user.Email;
            return View(_context.Cameras.ToList());
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View();

            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                bool isGoingToAdmin = !string.IsNullOrEmpty(returnUrl) && returnUrl.Contains("Admin");
                bool isAdminUser = await _userManager.IsInRoleAsync(user, "Admin");

                if (isAdminUser && !isGoingToAdmin)
                {
                    ModelState.AddModelError("", "Admins must log in via the Admin Panel.");
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName,
                    password,
                    isPersistent: rememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Dashboard");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View();
        }

        [HttpGet]
        public IActionResult Registration() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(User user, string password)
        {
            var newUser = new User
            {
                UserName = user.Email,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                InstagramUsername = user.InstagramUsername,
                Status = "Active",
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                return RedirectToAction("Dashboard");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync(); 

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Main");
        }
    }
}
