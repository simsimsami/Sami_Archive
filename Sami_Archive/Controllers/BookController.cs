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

        [HttpGet("Book/{id}")]
        public async Task<IActionResult> GetBook(long id)
        {
            var book = await _context.Books.FindAsync(id);
            return book == null ? NotFound() : Ok(book);
        }

        // POST: /Book/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBook), new { id = book.BookID }, book);
            }
            return View(book);
        }

        // PUT: /Book/Put
    }
}
