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
            var currentBook = await _context.Books.FindAsync(book.BookID);

            currentBook.Title = book.Title; 
            currentBook.Description = book.Description;
            currentBook.Genre = book.Genre;

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
