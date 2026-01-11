using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proj.Models;
using Proj.Data;

namespace Proj.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment; // For saving files

    public HomeController(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public IActionResult main() { return View(); }

    public IActionResult login() { return View(); }

    // <--- Add POST Login Logic --->
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        
        if (user != null)
        {
            return RedirectToAction("product");
        }
        return View();
    }

    public IActionResult register() { 
        return View(); 
    }

    // <--- Add POST Register Logic --->
    [HttpPost]
    public IActionResult Register(string email, string password, string confirmPassword)
    {
        if (password == confirmPassword)
        {
            var newUser = new User { Email = email, Password = password };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return RedirectToAction("login");
        }
        return View();
    }

public IActionResult product()
    {
        return View();
    }

    // GET: Form
    // Accepts parameters from the Product page
    public IActionResult form(string cameraName, decimal price)
    {
        // Pass these values to the view so the form knows what was selected
        ViewBag.CameraName = cameraName;
        ViewBag.Price = price;
        return View();
    }

    // POST: Handle the Rental Form Submission
    [HttpPost]
    public async Task<IActionResult> SubmitRental(RentalRequest request, IFormFile validIdFile, IFormFile paymentProofFile)
    {
        // 1. Handle File Uploads
        if (validIdFile != null)
        {
            string folder = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(folder); // Ensure folder exists
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(validIdFile.FileName);
            string filePath = Path.Combine(folder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await validIdFile.CopyToAsync(stream);
            }
            request.ValidIdPath = "/uploads/" + fileName;
        }

        if (paymentProofFile != null)
        {
            string folder = Path.Combine(_environment.WebRootPath, "uploads");
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(paymentProofFile.FileName);
            string filePath = Path.Combine(folder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await paymentProofFile.CopyToAsync(stream);
            }
            request.PaymentProofPath = "/uploads/" + fileName;
        }

        // 2. Save to Database
        _context.RentalRequests.Add(request);
        await _context.SaveChangesAsync();

        // 3. Redirect to Summary with the Request ID
        return RedirectToAction("summary", new { id = request.Id });
    }

    public IActionResult summary(int id)
    {
        // Fetch the request to show the confirmation details
        var request = _context.RentalRequests.Find(id);
        if (request == null) return RedirectToAction("product");

        return View(request);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
