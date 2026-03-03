using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sami_Archive.Models;
using Sami_Archive.Models.ViewModels;

namespace Sami_Archive.Controllers
{
    public class BookController : Controller
    {
        public int PageSize = 4;
        private readonly StoreDbContext _context;
        private IBookRepository repository;

        public BookController(StoreDbContext context, IBookRepository repo)
        {
            _context = context;
            repository = repo;
        }

        public ViewResult List(string? genre, int bookPage = 1, string? title = null)
        {
            return View(new BooksListViewModels
            {
                Books = repository.Books
                .Where(p => genre == null || p.Genre == genre)
                .Where(p => title == null || p.Title == title)
                .OrderBy(p => p.BookID)
                .Skip((bookPage - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = bookPage,
                    ItemsPerPage = PageSize,
                    TotalItems = genre == null ? repository.Books.Count() : repository.Books.Where(e => e.Genre == genre).Count()
                },
                CurrentGenre = genre,
                TitleFilter = title
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long? BookID)
        {
            var book = await _context.Books.FindAsync(BookID);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long? BookID, [Bind("BookID,Title,Description,Genre")] Book book)
        {
            if (BookID != book.BookID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "home");
                }
                catch (DbUpdateConcurrencyException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes... ");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // View result will help with PUT and READ

        public ViewResult Create() => View();

        // POST: /Book/Create
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("Index", "Home");
        }

    }
}
