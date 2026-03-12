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

        public ViewResult Index(int bookPage = 1, string? title = null)
        {
            var query = bookRepository.Books
                .Include(b => b.Genres)
                .Include(b => b.Authors)
                .Where(b => title == null || b.BookTitle == title);

            var totalItems = query.Count();

            var books = query
                .OrderBy(b => b.BookID)
                .Skip((bookPage - 1) * PageSize)
                .Take(PageSize);


            return View(new BooksListViewModels
            {
                Books = books,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = bookPage,
                    ItemsPerPage = PageSize,
                    TotalItems = totalItems
                },
                TitleFilter = title
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long? BookID)
        {
            if (BookID == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookID == BookID);
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
        public async Task<IActionResult> DeleteBook(long BookID)
        {
            try
            {
                var book = await _context.Books.FindAsync(BookID);
                if (book == null)
                {
                    return NotFound($"Book with ID = {BookID} not found");
                }
                await bookRepository.DeleteBookAsync(BookID);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }
    }
}
