using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Proj.Data;
using Proj.Models;
using Microsoft.EntityFrameworkCore;

namespace Proj.Controllers
{
    public class RentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public RentController(ApplicationDbContext context, UserManager<User> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Reserve(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var camera = await _context.Cameras.FindAsync(id);

            if (camera == null) return NotFound();

            ViewBag.FullName = user.FullName;
            ViewBag.Phone = user.PhoneNumber;
            ViewBag.Address = user.Address;
            ViewBag.Instagram = user.InstagramUsername;
            ViewBag.Email = user.Email;

            return View(new Reservation
            {
                CameraId = id,
                TotalPrice = camera.Price,
                UserId = user.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(Reservation reservation)
        {
            ModelState.Remove("User");
            ModelState.Remove("Camera");
            ModelState.Remove("Status");
            ModelState.Remove("ValidIdPath");
            ModelState.Remove("ProofOfPaymentPath");
            ModelState.Remove("Payments");

            if (ModelState.IsValid)
            {
                var camera = await _context.Cameras.FindAsync(reservation.CameraId);
                if (camera == null) return NotFound();

                int totalDays = (reservation.EndDate - reservation.StartDate).Days;
                reservation.TotalPrice = (totalDays <= 0 ? 1 : totalDays) * camera.Price;

                if (reservation.ValidIdImage != null)
                {
                    reservation.ValidIdPath = await SaveFile(reservation.ValidIdImage);
                }

                if (reservation.ProofOfPaymentImage != null)
                {
                    reservation.ProofOfPaymentPath = await SaveFile(reservation.ProofOfPaymentImage);
                }

                reservation.Status = "Pending";
                reservation.Id = 0;

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Reservation Submitted Successfully!";
                return RedirectToAction("Dashboard", "Home");
            }

            var user = await _userManager.GetUserAsync(User);
            ViewBag.FullName = user.FullName;
            ViewBag.Phone = user.PhoneNumber;
            ViewBag.Address = user.Address;
            ViewBag.Instagram = user.InstagramUsername;
            ViewBag.Email = user.Email;

            return View(reservation);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            string uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "images", "uploads");
            if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

            string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            string filePath = Path.Combine(uploadDir, fileName);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }
            return fileName;
        }
    }
}
