using Microsoft.EntityFrameworkCore;

namespace Sami_Archive.Models
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            StoreDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book
                    {
                        Title = "Necromancer Supreme Vol 1", Description = "Introduction to the first necromancer", Genre = "Fantasy"
                    });
                context.SaveChanges();
            }
        }
    }
}
