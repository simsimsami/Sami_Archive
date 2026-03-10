using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sami_Archive.Models;
using Sami_Archive.Models.ViewModels;

namespace Sami_Archive.Controllers
{
    public class HomeController : Controller
    {
        // The home controller is the library
        private IBookRepository bookRepository;

        public HomeController(IBookRepository repo)
        {
            bookRepository = repo;
        }

        public ViewResult Index()
        {
            return View();
        }
    }
}
