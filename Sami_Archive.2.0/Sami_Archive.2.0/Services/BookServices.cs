using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Sami_Archive._2._0.Data;
using Sami_Archive._2._0.Models;
using System.Linq.Expressions;
using System.Net;


namespace Sami_Archive._2._0.Services
{
    public class BookServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public BookServices(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            await using var context = await _factory.CreateDbContextAsync();
            return await context.Books.ToListAsync();
        }

        public async Task AddAsync(Book book)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();
                context.Books.Add(book);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> EditAsync(Book book)
        {
            try
            {

                await using var context = await _factory.CreateDbContextAsync();
                // Find the specific book in question

                var bookID = book.BookID;

                var bookToEdit = await context.Books.FirstOrDefaultAsync(x => x.BookID == bookID);

                if (bookToEdit == null)
                {
                    return false;
                }

                bookToEdit.BookTitle = book.BookTitle;
                bookToEdit.BookDescription = book.BookDescription;

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int BookID)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();

                var bookToDelete = await context.Books.FirstOrDefaultAsync(x => x.BookID == BookID);

                if (bookToDelete == null)
                {
                    return false;
                }

                context.Books.Remove(bookToDelete);

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
