using Microsoft.AspNetCore.Mvc;
using Sami_Archive.Models;
using Sami_Archive.Models.ViewModels;

namespace Sami_Archive.Controllers
{
    public class BookController : Controller
    {
        private readonly StoreDbContext _context;

        public BookController(StoreDbContext context)
        {
            _context = context;
        } 


        public async Task<IActionResult> Edit(long id)
        {
            var book = await _context.Books.FindAsync(id);
            return book == null ? NotFound() : View(book);
        }

        // View result will help with PUT and READ

        public ViewResult Create() => View();

        // POST: /Book/Create
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                return View(book); 
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Create), new { id = book.BookID });
        }

        
    }
}
