using Microsoft.AspNetCore.Mvc;
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

        public int PageSize = 4;

        public ViewResult Index(string? genre, int bookPage = 1, string? title = null)
        {
            return View(new BooksListViewModels
            {
                Books = bookRepository.Books
                .Where(p => genre == null || p.Genre == genre)
                .Where(p => title == null || p.Title == title)
                .OrderBy(p => p.BookID)
                .Skip((bookPage - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = bookPage,
                    ItemsPerPage = PageSize,
                    TotalItems = genre == null ? bookRepository.Books.Count() : bookRepository.Books.Where(e => e.Genre == genre).Count()
                },
                CurrentGenre = genre,
                TitleFilter = title
            });
        }
    }
}
