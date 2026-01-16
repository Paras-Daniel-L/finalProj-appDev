using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proj.Data;
using Proj.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Proj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminController(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Dashboard()
        {
            var reservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Camera)
                .OrderByDescending(r => r.Id)
                .ToListAsync();

            ViewBag.TotalReservations = reservations.Count;
            ViewBag.PendingApproval = reservations.Count(r => r.Status == "Pending");
            ViewBag.ActiveRentals = reservations.Count(r => r.Status == "Approved" || r.Status == "Active");
            ViewBag.TotalRevenue = reservations.Sum(r => r.TotalPrice);

            return View(reservations);
        }

        public async Task<IActionResult> Reservations(string searchString)
        {
            var query = _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Camera)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(r =>
                    r.User.FullName.Contains(searchString) ||
                    r.Id.ToString().Contains(searchString));
            }

            return View(await query.OrderByDescending(r => r.Id).ToListAsync());
        }

        public async Task<IActionResult> ReservationDetails(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Camera)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Camera)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            var current = reservation.Status ?? "Pending";

            if (current == "Pending" && (status == "Approved" || status == "Rejected"))
            {
                reservation.Status = status;

                if (status == "Approved" && reservation.Camera != null)
                    reservation.Camera.IsAvailable = false;
            }
            else if (current == "Approved" && status == "Active")
            {
                reservation.Status = "Active";
            }
            else if ((current == "Active" || current == "Approved") && status == "Completed")
            {
                reservation.Status = "Completed";

                if (reservation.Camera != null)
                    reservation.Camera.IsAvailable = true;
            }
            else if (status == "Cancelled" && current == "Pending")
            {
                reservation.Status = "Cancelled";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Reservations));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPayment(int id)
        {
            var res = await _context.Reservations.FindAsync(id);
            if (res == null)
                return NotFound();

            if (!string.IsNullOrEmpty(res.ProofOfPaymentPath) && res.Status == "Pending")
                res.Status = "Approved";

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Reservations));
        }

        public async Task<IActionResult> Equipment()
        {
            return View(await _context.Cameras.ToListAsync());
        }

        public IActionResult CreateCamera() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCamera(Camera model, IFormFile ImageFile)
        {
            if (ImageFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "images", fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                model.ImageUrl = fileName;
                model.IsAvailable = true;

                _context.Cameras.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Equipment");
            }
            return View(model);
        }

        public async Task<IActionResult> EditCamera(int id)
        {
            var camera = await _context.Cameras.FindAsync(id);
            if (camera == null) return NotFound();
            return View(camera);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCamera(Camera model, IFormFile? ImageFile)
        {
            var camera = await _context.Cameras.FindAsync(model.Id);
            if (camera == null) return NotFound();

            camera.Name = model.Name;
            camera.Brand = model.Brand;
            camera.Model = model.Model;
            camera.Description = model.Description;
            camera.Price = model.Price;
            camera.IsAvailable = model.IsAvailable;

            if (ImageFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "images", fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                camera.ImageUrl = fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Equipment");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCamera(int id)
        {
            var camera = await _context.Cameras.FindAsync(id);
            if (camera != null)
            {
                _context.Cameras.Remove(camera);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Equipment");
        }

        public async Task<IActionResult> Customers(string searchString)
        {
            var query = _context.Users
                .Include(u => u.Reservations)
                .Where(u => u.Email != "admin@appdev.com")
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(u => u.FullName.Contains(searchString) || u.Email.Contains(searchString));
            }

            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> Payments()
        {
            var res = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Camera)
                .OrderByDescending(r => r.Id)
                .ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> Settings()
        {
            return View(await _userManager.GetUserAsync(User));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string FullName, string Email)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.FullName = FullName;
                user.Email = Email;
                user.UserName = Email;
                await _userManager.UpdateAsync(user);
                TempData["Success"] = "Profile updated!";
            }
            return RedirectToAction("Settings");
        }
    }
}
