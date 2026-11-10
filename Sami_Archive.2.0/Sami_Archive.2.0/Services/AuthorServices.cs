using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Sami_Archive._2._0.Data;
using Sami_Archive._2._0.Models;
using System.Net;

namespace Sami_Archive._2._0.Services
{
    public class AuthorServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public AuthorServices(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<Author>> GetAllAsync()
        {
            await using var context = await _factory.CreateDbContextAsync();
            return await context.Authors.ToListAsync();
        }

        public async Task<bool> AddAsync(Author author)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();
                context.Authors.Add(author);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> EditAsync(Author author)
        {
            try
            {
                using var context = await _factory.CreateDbContextAsync();

                var authorID = author.AuthorID;

                var authorToEdit = await context.Authors.FirstOrDefaultAsync(x => x.AuthorID == authorID);

                if (authorToEdit == null)
                {
                    return false;
                }

                authorToEdit.AuthorName = author.AuthorName;

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long AuthorID)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();
                var authorToDelete = await context.Authors.FirstOrDefaultAsync(x => x.AuthorID == AuthorID);

                if (authorToDelete == null)
                {
                    return false;
                }

                context.Authors.Remove(authorToDelete);
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
