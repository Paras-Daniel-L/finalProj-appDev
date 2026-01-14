using Microsoft.AspNetCore.Mvc;
using Proj.Data;
using Proj.Models;
using System.Diagnostics;

namespace Proj.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult main()
    {
        var cameras = _context.Cameras.ToList();
        return View(cameras);
    }

    public IActionResult login()
    {
        return View();
    }

    public IActionResult product()
    {
        return View();
    }

    public IActionResult registration()
    {
        return View();
    }

    public IActionResult summary()
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
