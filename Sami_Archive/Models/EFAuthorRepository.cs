using Microsoft.EntityFrameworkCore;

namespace Sami_Archive.Models
{
    public class EFAuthorRepository : IAuthorRepository
    {
        private StoreDbContext _context;
        public EFAuthorRepository(StoreDbContext context)
        {
            _context = context;
        }
        public IQueryable<Author> Authors => _context.Authors;
        public async Task AddAuthorAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            var currentAuthor = await _context.Authors
                .FirstOrDefaultAsync(a => a.AuthorID == author.AuthorID);

            if (currentAuthor == null) { return; }

            // Update scalar properties
            currentAuthor.AuthorName = author.AuthorName;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuthorAsync(long AuthorID)
        {
            var author = await _context.Authors.FindAsync(AuthorID);
            _context.Remove(author);
            await _context.SaveChangesAsync();
        }
    }
}
