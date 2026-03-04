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
        private IBookRepository bookRepository;

        public BookController(StoreDbContext context, IBookRepository repo)
        {
            _context = context;
            bookRepository = repo;
        }

        public ViewResult List(string? genre = null, int bookPage = 1, string? title = null)
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
            if (ModelState.IsValid)
            {
                try
                {
                    await bookRepository.UpdateBookAsync(book);

                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes... ");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }
            try
            {

                await bookRepository.AddBookAsync(book);
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to create book");
                return View(book);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteForm(long? BookID)
        {
            var book = _context.Books.Find(BookID);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBook(long? BookID)
        {
            try
            {
                var book = await _context.Books.FindAsync(BookID);
                if (book == null)
                {
                    return NotFound($"Book with ID = {BookID} not found");
                }
                await bookRepository.DeleteBookAsync(book);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }
    }
}
