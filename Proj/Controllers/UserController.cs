using Microsoft.AspNetCore.Mvc;

namespace Proj.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
