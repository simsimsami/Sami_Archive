using Microsoft.EntityFrameworkCore;
using Sami_Archive._2._0.Data;
using Sami_Archive._2._0.Models;

namespace Sami_Archive._2._0.Services
{
    public class CommentService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public CommentService(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<List>> GetAllAsync()
        {
            await using var context = await _factory.CreateDbContextAsync();
            return await context.Lists.ToListAsync();
        }

        public async Task<bool> AddAsync(UserBookComment comment)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();
                context.Add(comment);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<bool> EditAsync(UserBookComment comment)
        //{
        //    try
        //    {
        //        await using var context = await _factory.CreateDbContextAsync();
        //        //var commentID = comment.
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}
