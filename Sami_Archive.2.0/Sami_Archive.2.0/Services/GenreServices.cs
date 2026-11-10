using Microsoft.EntityFrameworkCore;
using Sami_Archive._2._0.Data;
using Sami_Archive._2._0.Models;
using System.Linq.Expressions;

namespace Sami_Archive._2._0.Services
{
    public class GenreServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public GenreServices(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<Genre>> GetAllAsync()
        {
            await using var context = await _factory.CreateDbContextAsync();
            return await context.Genres.ToListAsync();
        }

        public async Task<bool> AddAsync(Genre genre)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();
                context.Genres.Add(genre);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> EditAsync(Genre genre)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();
                var genreID = genre.GenreID;
                var genreToEdit = await context.Genres.FirstOrDefaultAsync(x => x.GenreID == genreID);

                if (genreToEdit == null)
                {
                    return false;
                }

                genreToEdit.GenreName = genre.GenreName;

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long GenreID)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();
                var authorToDelete = await context.Genres.FirstOrDefaultAsync(x => x.GenreID == GenreID);

                if (authorToDelete == null)
                {
                    return false;
                }

                context.Remove(authorToDelete);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
