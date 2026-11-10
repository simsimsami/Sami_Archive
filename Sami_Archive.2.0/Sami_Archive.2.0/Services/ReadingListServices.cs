using Microsoft.EntityFrameworkCore;
using Sami_Archive._2._0.Data;
using Sami_Archive._2._0.Models;

namespace Sami_Archive._2._0.Services
{
    public class ReadingListServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public ReadingListServices(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<List>> GetAllAsync()
        {
            await using var context = await _factory.CreateDbContextAsync();
            return await context.Lists.ToListAsync();
        }

        public async Task<bool> AddAsync(List list)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();
                context.Lists.Add(list);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> EditAsync(List list)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();
                var listID = list.ListID;

                var listToEdit = await context.Lists.FirstOrDefaultAsync(x => x.ListID == listID);

                if (listToEdit == null)
                {
                    return false;
                }

                listToEdit.ListName = list.ListName;
                listToEdit.ListUser = list.ListUser;
                listToEdit.ListingBooks = list.ListingBooks;

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int ListID)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();
                var listToDelete = await context.Lists.FirstOrDefaultAsync(x => x.ListID == ListID);

                if (listToDelete == null)
                {
                    return false;
                }

                context.Remove(listToDelete);
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
