using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proj.Models;

namespace Proj.Controllers;

public class HomeController : Controller
{
    public IActionResult main()
    {
        return View();
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
