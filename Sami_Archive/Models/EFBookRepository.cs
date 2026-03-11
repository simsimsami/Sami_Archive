using Microsoft.EntityFrameworkCore;

namespace Sami_Archive.Models
{
    public class EFBookRepository : IBookRepository
    {
        private StoreDbContext _context;
        public EFBookRepository(StoreDbContext ctx)
        {
            _context = ctx;
        }
        public IQueryable<Book> Books => _context.Books;
        public async Task AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateBookAsync(Book book)
        {
            var currentBook = await _context.Books
                .Include(b => b.Genres)
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.BookID == book.BookID);

            if (currentBook == null) { return; }

            // Update Scalar properties
            currentBook.BookTitle = book.BookTitle;
            currentBook.BookDescription = book.BookDescription;

            // Update genres
            currentBook.Genres.Clear();
            foreach(var genre in book.Genres)
            {
                _context.Attach(genre);
                currentBook.Genres.Add(genre);
            }

            currentBook.Authors.Clear();
            foreach(var author in book.Authors)
            {
                _context.Attach(author);
                currentBook.Authors.Add(author);
            }

            await _context.SaveChangesAsync();
        }
        public async Task DeleteBookAsync(long BookID)
        {
            var book = await _context.Books.FindAsync(BookID);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
