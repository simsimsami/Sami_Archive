using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sami_Archive.Models;
using Sami_Archive.Models.ViewModels;

namespace Sami_Archive.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // The home controller is the library
        private IBookRepository bookRepository;

        public HomeController(IBookRepository repo)
        {
            bookRepository = repo;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
