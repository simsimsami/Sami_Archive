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
                        Title = "Necromancer Supreme Vol 1",
                        Description = "Introduction to the first necromancer",
                        Genre = "Fantasy",
                    },
                    new Book
                    {
                        Title = "Farmers Love",
                        Description = "Farming and the great outdoors",
                        Genre = "Drama"
                    },
                    new Book
                    {
                        Title = "John Q",
                        Description = "A colorful variety of crunchy veggie chips for snacking.",
                        Genre = "Crime"
                    },
                    new Book
                    {
                        Title = "Vow, The",
                        Description = "Vegetable spiralizer for healthy meals.",
                        Genre = "Drama"
                    },
                    new Book
                    {
                        Title = "25 Watts",
                        Description = "GPS pet collar that helps locate your pet via smartphone app.",
                        Genre = "Comedy"
                    },
                    new Book
                    {
                        Title = "Mustalaishurmaaja",
                        Description = "Cute dispenser for bathrooms or kitchens featuring paw prints.",
                        Genre = "Drama"
                    },
                    new Book
                    {
                        Title = "16 Years of Alcohol",
                        Description = "Creamy macaroni and cheese baked to perfection.",
                        Genre = "Drama"
                    },
                    new Book
                    {
                        Title = "Por un puñado de besos",
                        Description = "Elegant cover that protects your notebook with style.",
                        Genre = "Drama"
                    },
                    new Book
                    {
                        Title = "Gertie the Dinosaur",
                        Description = "Golden-brown fritters made with sweet corn.",
                        Genre = "Animation"
                    },
                    new Book
                    {
                        Title = "Goats",
                        Description = "Delicious ravioli filled with creamy ricotta and fresh spinach.",
                        Genre = "Comedy"
                    },
                    new Book
                    {
                        Title = "Page Turner, The (Tourneuse de pages, La)",
                        Description = "Color-changing 3D night light for kids' rooms.",
                        Genre = "Drama"
                    },
                    new Book
                    {
                        Title = "God Bless America",
                        Description = "Complete kit to make your own flavored lip balms at home.",
                        Genre = "Comedy"
                    },
                    new Book
                    {
                        Title = "Promised Land",
                        Description = "Countertop dishwasher for small kitchens.",
                        Genre = "Drama"
                    },
                    new Book
                    {
                        Title = "Abbott and Costello Meet Dr. Jekyll and Mr. Hyde",
                        Description = "Complete kit for starting your own organic garden.",
                        Genre = "Comedy"
                    },
                    new Book
                    {
                        Title = "Love Actually",
                        Description = "Seat belt attachment to keep dogs safe during car rides.",
                        Genre = "Comedy"
                    },
                    new Book
                    {
                        Title = "Critical Care",
                        Description = "Sweet corn roasted to perfection for a delightful side.",
                        Genre = "Comedy"
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
