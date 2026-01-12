using Microsoft.AspNetCore.Mvc;

namespace Proj.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
