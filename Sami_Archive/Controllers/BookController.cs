using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sami_Archive.Models;
using Sami_Archive.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Index(int page = 1, string? title = null)
        {
            var query = bookRepository.Books
                .Include(b => b.Genres)
                .Include(b => b.Authors)
                .Where(b => title == null || b.BookTitle == title);

            var totalItems = query.Count();

            var books = query
                .OrderBy(b => b.BookID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);


            return View(new BooksListViewModels
            {
                Books = books,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = totalItems
                },
                TitleFilter = title
            });
        }

        public IActionResult Create() {

            var CreateBookVM = new CreateBookViewModel
            {
                Authors = _context.Authors
                .Select(a => new KeyValuePair<long, string>(a.AuthorID, a.AuthorName))
                .ToList(),

                Genres = _context.Genres
                .Select(g => new KeyValuePair<long, string>(g.GenreID, g.GenreTitle))
                .ToList()
            };

            return View(CreateBookVM); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookViewModel book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await bookRepository.AddBookAsync(book);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            ModelState.AddModelError("", "Error in Book ModelState");
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long BookID)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookID == BookID);
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long BookID, [Bind("BookID,Title,Description,Genre")] Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await bookRepository.UpdateBookAsync(book);
                    return RedirectToAction("Index", "Book");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            ModelState.AddModelError("", "Unable to save changes... ");
            return RedirectToAction("Index", "Book");
        }


        [HttpGet]
        public async Task<IActionResult> DeleteForm(long BookID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await bookRepository.DeleteBookAsync(BookID);
                    return RedirectToAction("Index", "Book");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            ModelState.AddModelError("", "Error in Book ModelState");
            return RedirectToAction("Index", "Book");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBook(long BookID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var book = await _context.Books.FindAsync(BookID);
                    if (book == null) return NotFound($"Book with ID = {BookID} not found");

                    await bookRepository.DeleteBookAsync(BookID);
                    return RedirectToAction("Index", "Book");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            ModelState.AddModelError("", "Unable to save changes... ");
            return RedirectToAction("Index", "Home");
        }
    }
}
