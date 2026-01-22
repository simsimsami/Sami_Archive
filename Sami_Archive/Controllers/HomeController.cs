using Microsoft.AspNetCore.Mvc;
using Sami_Archive.Models;

namespace Sami_Archieve.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository repository;

        public HomeController(IStoreRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index() => View(repository.Products);
    }
}
