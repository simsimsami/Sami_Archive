using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sami_Archive.Models;
using Sami_Archive.Models.ViewModels;
using System.Net;

namespace Sami_Archive.Controllers
{
    public class BookController : Controller
    {
        public int PageSize = 10;
        private readonly StoreDbContext _context;
        private readonly IBookRepository bookRepository;
        private readonly IGenreRepository genreRepository;


        public BookController(StoreDbContext context, IBookRepository _bookRepository, IGenreRepository _genreRepository)
        {
            _context = context;
            bookRepository = _bookRepository;
            genreRepository = _genreRepository;
        }

        public IActionResult Index(int page = 1, string? title = null, List<string>? selectedGenres = null, List<string>? selectedAuthors = null)
        {
            selectedGenres ??= new List<string>();
            selectedAuthors ??= new List<string>();

            var query = bookRepository.Books
                .Include(b => b.Genres)
                .Include(b => b.Authors)
                .Where(b => title == null || b.BookTitle == title)
                .Where(b => !selectedGenres.Any() || b.Genres.Any(g => selectedGenres.Contains(g.GenreTitle)))
                .Where(b => !selectedAuthors.Any() || b.Authors.Any(a => selectedAuthors.Contains(a.AuthorName)));

            var totalItems = query.Count();

            var books = query
                .OrderBy(b => b.BookID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            var genres = genreRepository.Genres;

            return View(new BooksListViewModels
            {
                Books = books,
                Genres = genres,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = totalItems
                },
                TitleFilter = title,
                GenreFilter = selectedGenres,
                AuthorFilter = selectedAuthors
            });
        }

        public IActionResult Create()
        {
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await bookRepository.AddBookAsync(viewModel);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }

            viewModel.Authors = _context.Authors
                .Select(a => new KeyValuePair<long, string>(a.AuthorID, a.AuthorName))
                .ToList();
            viewModel.Genres = _context.Genres
                .Select(g => new KeyValuePair<long, string>(g.GenreID, g.GenreTitle))
                .ToList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(long BookID)
        {
            /// I want the user to "click" on the box of the book, and it presents details of the book to the user
            /// I might need a method to capture the input from the user, compare the input to the database on what the user wants to see
            /// Get the ID, present it as a view to the user and place it onto a page.
            /// 
            /// The books class already contains authors, genres and itself. Just get the idea of what the user pressed, search for it, then present it as a view
            /// 
            var bookDetails = _context.Books.FirstOrDefault(b => b.BookID == BookID);

            if (bookDetails == null)
            {
                return NotFound();
            }

            return View(bookDetails);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(long BookID)
        {
            var bookDetails = _context.Books
                .Select(row => new
                {
                    Book = row,
                    Authors = row.Authors.Select(a => a.AuthorID).ToList(),
                    Genres = row.Genres.Select(g => g.GenreID).ToList(),
                })
                .FirstOrDefault(b => b.Book.BookID == BookID);

            if (bookDetails == null)
            {
                return NotFound();
            }

            var CreateBookVM = new UpdateBooksViewModel
            {
                BookID = BookID,
                BookTitle = bookDetails.Book.BookTitle,
                BookDescription = bookDetails.Book.BookDescription,
                Authors = _context.Authors
                .Select(a => new KeyValuePair<long, string>(a.AuthorID, a.AuthorName))
                .ToList(),

                Genres = _context.Genres
                .Select(g => new KeyValuePair<long, string>(g.GenreID, g.GenreTitle))
                .ToList(),
                SelectedAuthors = bookDetails.Authors,
                SelectedGenres = bookDetails.Genres
            };

            return View(CreateBookVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateBooksViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await bookRepository.UpdateBookAsync(viewModel);
                    return RedirectToAction("Index", "Book");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }


            viewModel.Authors = _context.Authors
                .Select(a => new KeyValuePair<long, string>(a.AuthorID, a.AuthorName))
                .ToList();
            viewModel.Genres = _context.Genres
                .Select(g => new KeyValuePair<long, string>(g.GenreID, g.GenreTitle))
                .ToList();

            return View(viewModel);
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
        [ValidateAntiForgeryToken]
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