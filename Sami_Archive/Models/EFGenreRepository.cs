using Microsoft.EntityFrameworkCore;

namespace Sami_Archive.Models
{
    public class EFGenreRepository : IGenreRepository
    {
        private StoreDbContext _context;
        public EFGenreRepository(StoreDbContext ctx)
        {
            _context = ctx;
        }
        public IQueryable<Genre> Genres => _context.Genres;
        public async Task AddGenreAsync(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateGenreAsync(Genre genre)
        {
            var currentGenre = await _context.Genres
                .Include(g => g.GenreTitle)
                .FirstOrDefaultAsync(g => g.GenreID == genre.GenreID);

            if (currentGenre == null) { return; }

            // Update scalar properties
            currentGenre.GenreTitle = genre.GenreTitle;

            await _context.SaveChangesAsync();
        }
        public async Task DeleteGenreAsync(long GenreID)
        {
            var genre = await _context.Genres.FindAsync(GenreID);
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }
    }
}
