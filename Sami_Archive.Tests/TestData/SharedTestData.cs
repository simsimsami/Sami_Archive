using Microsoft.EntityFrameworkCore;
using Moq;
using Sami_Archive.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sami_Archive.Tests.TestData
{
    public class SharedTestData
    {
        public static Mock<IBookRepository> BookMockRepo()
        {
            var mock = new Mock<IBookRepository>();

            var bookOne = new Book { BookID = 1, BookTitle = "B1", BookDescription = "D1" };
            bookOne.Genres.Add(new Genre { GenreID = 1, GenreTitle = "G1" });
            bookOne.Authors.Add(new Author { AuthorID = 1, AuthorName = "A1" });

            var books = new List<Book>
            {
                new Book { BookID = 1, BookTitle = "B1", BookDescription = "D1" },
                new Book { BookID = 2, BookTitle = "B2", BookDescription = "D2" },
                new Book { BookID = 3, BookTitle = "B3", BookDescription = "D3" },
                new Book { BookID = 4, BookTitle = "B4", BookDescription = "D4" },
                new Book { BookID = 5, BookTitle = "B5", BookDescription = "D5" },
            };

            mock.Setup(m => m.Books).Returns(books.AsQueryable());


            return mock;
        }

        public static Mock<IAuthorRepository> AuthorMockRepo()
        {
            var mock = new Mock<IAuthorRepository>();

            mock.Setup(m => m.Authors).Returns((new List<Author>
            {
                new Author { AuthorID = 1, AuthorName = "AN1" },
                new Author { AuthorID = 2, AuthorName = "AN2" },
                new Author { AuthorID = 3, AuthorName = "AN3" },
                new Author { AuthorID = 4, AuthorName = "AN4" },
                new Author { AuthorID = 5, AuthorName = "AN5" },
            }).AsQueryable());

            return mock;
        }

        public static Mock<IGenreRepository> GenreMockRepo()
        {
            var mock = new Mock<IGenreRepository>();

            mock.Setup(m => m.Genres).Returns((new List<Genre>
            {
                new Genre { GenreID = 1, GenreTitle = "G1" },
                new Genre { GenreID = 2, GenreTitle = "G2" },
                new Genre { GenreID = 3, GenreTitle = "G3" },
                new Genre { GenreID = 4, GenreTitle = "G4" },
            }).AsQueryable());

            return mock;
        }

        public static StoreDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
                .Options;
            return new StoreDbContext(options);
        }
    }
}
