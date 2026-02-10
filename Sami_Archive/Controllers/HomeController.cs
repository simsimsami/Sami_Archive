using Microsoft.AspNetCore.Mvc;
using Sami_Archive.Models;

namespace Sami_Archieve.Controllers
{
    public class HomeController : Controller
    {
        private IBookRepository repository;

        public HomeController(IBookRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index() => View(repository.Books);
    }
}
