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

            if (!context.Genres.Any())
            {
                context.Genres.AddRange(
                    new Genre
                    {
                        GenreTitle = "Fantasy"
                    },
                    new Genre
                    {
                        GenreTitle = "Sci Fi"
                    },
                    new Genre
                    {
                        GenreTitle = "Comedy"
                    },
                    new Genre
                    {
                        GenreTitle = "Drama"
                    },
                    new Genre
                    {
                        GenreTitle = "Anime"
                    },
                    new Genre
                    {
                        GenreTitle = "Blank"
                    }
                    );
                context.SaveChanges();
            }


            if (!context.Authors.Any())
            {
                context.Authors.AddRange(
                    new Author
                    {
                        AuthorName = "Blank",
                    }
                    );
                context.SaveChanges();

            }


            if (!context.Books.Any())
            {
                var genreBlank = context.Genres.First(g => g.GenreTitle == "Blank");
                var authorBlank = context.Authors.First(g => g.AuthorName == "Blank");

                context.Books.AddRange(
                    new Book
                    {
                        BookTitle = "Necromancer Supreme Vol 1",
                        BookDescription = "Introduction to the first necromancer",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "Farmers Love",
                        BookDescription = "Farming and the great outdoors",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "John Q",
                        BookDescription = "A colorful variety of crunchy veggie chips for snacking.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "Vow, The",
                        BookDescription = "Vegetable spiralizer for healthy meals.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "25 Watts",
                        BookDescription = "GPS pet collar that helps locate your pet via smartphone app.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "Mustalaishurmaaja",
                        BookDescription = "Cute dispenser for bathrooms or kitchens featuring paw prints.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "16 Years of Alcohol",
                        BookDescription = "Creamy macaroni and cheese baked to perfection.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "Por un puñado de besos",
                        BookDescription = "Elegant cover that protects your notebook with style.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "Gertie the Dinosaur",
                        BookDescription = "Golden-brown fritters made with sweet corn.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "Goats",
                        BookDescription = "Delicious ravioli filled with creamy ricotta and fresh spinach.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "Page Turner, The (Tourneuse de pages, La)",
                        BookDescription = "Color-changing 3D night light for kids' rooms.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "God Bless America",
                        BookDescription = "Complete kit to make your own flavored lip balms at home.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "Promised Land",
                        BookDescription = "Countertop dishwasher for small kitchens.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "Abbott and Costello Meet Dr. Jekyll and Mr. Hyde",
                        BookDescription = "Complete kit for starting your own organic garden.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "Love Actually",
                        BookDescription = "Seat belt attachment to keep dogs safe during car rides.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    },
                    new Book
                    {
                        BookTitle = "Critical Care",
                        BookDescription = "Sweet corn roasted to perfection for a delightful side.",
                        Genres = { genreBlank },
                        Authors = { authorBlank }
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
