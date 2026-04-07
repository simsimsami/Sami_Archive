using Microsoft.EntityFrameworkCore;
using Sami_Archive.Models.ViewModels;
using System.Linq.Expressions;

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
        public async Task AddBookAsync(CreateBookViewModel viewModel)
        {
            var book = new Book
            {
                BookTitle = viewModel.BookTitle,
                BookDescription = viewModel.BookDescription,
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            if (viewModel.SelectedGenres?.Any() == true)
            {
                var selectedGenreIdsLong = viewModel.SelectedGenres.Select(i => (long)i).ToList();
                var genres = await _context.Genres
                    .Where(g => selectedGenreIdsLong.Contains(g.GenreID))
                    .ToListAsync();

                foreach(var item in genres)
                {
                    book.Genres.Add(item);
                }
            }

            if (viewModel.SelectedAuthors?.Any() == true)
            {
                var selectedAuthorIdsLong = viewModel.SelectedAuthors.Select(i => (long)i).ToList();
                var authors = await _context.Authors
                    .Where(a => selectedAuthorIdsLong.Contains(a.AuthorID))
                    .ToListAsync();

                foreach (var a in authors)
                {
                    book.Authors.Add(a);
                }
            }

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
            if (book == null) { return; }
            _context.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
