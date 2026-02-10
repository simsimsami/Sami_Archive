using Microsoft.EntityFrameworkCore;

namespace Sami_Archive.Models
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();
    }
}
