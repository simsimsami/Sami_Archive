using Microsoft.AspNetCore.Mvc;

namespace Sami_Archieve.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
