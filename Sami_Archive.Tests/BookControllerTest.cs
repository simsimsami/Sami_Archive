using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;
using Xunit.Sdk;
using Sami_Archive.Models;
using Sami_Archive.Controllers;
using Sami_Archive.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Reflection.Metadata.Ecma335;

namespace Sami_Archive.Tests
{
    public class BookControllerTest
    {
        public static Mock<IBookRepository> CreateMockRepo()
        {
            var mock = new Mock<IBookRepository>();

            mock.Setup(m => m.Books).Returns((new List<Book>
            {
                new Book { BookID = 1, BookTitle = "B1", BookDescription = "D1", Genres = { new Genre { GenreID = 1, GenreTitle = "G1" } } },
                new Book { BookID = 2, BookTitle = "B2", BookDescription = "D2", Genres = { new Genre { GenreID = 2, GenreTitle = "G2" } } },
                new Book { BookID = 3, BookTitle = "B3", BookDescription = "D3", Genres = { new Genre { GenreID = 3, GenreTitle = "G3" } } },
                new Book { BookID = 4, BookTitle = "B4", BookDescription = "D4", Genres = { new Genre { GenreID = 4, GenreTitle = "G4" } } },
                new Book { BookID = 5, BookTitle = "B5", BookDescription = "D5", Genres = { new Genre { GenreID = 5, GenreTitle = "G5" } } },
            }).AsQueryable());

            return mock;
        }
        private StoreDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
                .Options;
            return new StoreDbContext(options);
        }
        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange the mock data
            var mock = BookControllerTest.CreateMockRepo();

            // Arrange the controller and page size
            BookController controller = new BookController(null, mock.Object) { PageSize = 2 };

            // Act - declare a view model
            BooksListViewModels result = controller.List(1)?.ViewData.Model as BooksListViewModels ?? new();

            // Assert the pagination view model

            PagingInfo pageInfo = result.PagingInfo;
            Assert.NotNull(pageInfo);
            Assert.Equal(1, pageInfo.CurrentPage);
            Assert.NotEqual(3, pageInfo.ItemsPerPage);
            Assert.Equal(2, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(3, pageInfo.TotalPages);

        }
        [Fact]
        public void Can_Paginate()
        {
            // Arrange - Declaring the object mock - giving it mock data.
            var mock = BookControllerTest.CreateMockRepo();

            BookController controller = new BookController(null, mock.Object) { PageSize = 3 };

            // Act - no filters, looking at the second page
            BooksListViewModels result = controller.List(1)?.ViewData.Model as BooksListViewModels ?? new();

            Book[] bookArray = result.Books.ToArray();
            Assert.True(bookArray.Length == 3);
            Assert.Equal("B1", bookArray[0].BookTitle);
            Assert.Equal("B2", bookArray[1].BookTitle);
            Assert.NotEqual("B3", bookArray[1].BookTitle);
        }

        [Fact]
        public void Can_Access_Repository()
        {
            // Arrange - mock data
            var mock = BookControllerTest.CreateMockRepo();

            BookController controller = new BookController(null, mock.Object) { PageSize = 3 };

            // Act - getting access to the repo
            BooksListViewModels result = controller.List(2)?.ViewData.Model as BooksListViewModels ?? new();

            // Assert - checking if the controller can access the bookRepository
            Book[] bookArray = result.Books.ToArray();
            Assert.True(bookArray.Length == 2);
            Assert.NotNull(bookArray);

        }

        [Fact]
        public void Can_Filter_Books()
        {
            // Arrange - setup mock repo
            var mock = BookControllerTest.CreateMockRepo();

            // Arrange - setup controller
            BookController controller = new BookController(null, mock.Object);
            controller.PageSize = 3;

            // Fix filtering system
        }

        private BookController CreateController(StoreDbContext context)
        {
            var repo = new EFBookRepository(context);
            return new BookController(context, repo);
        }

        [Fact]
        public async Task CreateBook_WhenValid()
        {
            // Arrange
            var context = CreateDbContext();
            BookController controller = CreateController(context);

            Book newBook = new Book
            {
                BookID = 1,
                BookTitle = "Test Title",
                BookDescription = "Test Description",
                Genres = { new Genre { GenreID = 1, GenreTitle = "Test" } }
            };

            // Act
            var result = await controller.Create(newBook);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            var saved = await context.Books.FirstAsync();
            Assert.Equal("Test Title", saved.BookTitle);
        }

        [Fact]
        public async Task UpdateBook_WhenValid()
        {
            // Arrange
            var context = CreateDbContext();
            BookController controller = CreateController(context);

            Book newBook = new Book
            {
                BookID = 1,
                BookTitle = "Test Title 1",
                BookDescription = "Test Description 1",
                Genres = { new Genre { GenreID = 1, GenreTitle = "Test1" } }
            };

            Book editBook = new Book
            {
                BookID = 1,
                BookTitle = "Test Title 2",
                BookDescription = "Test Description 2",
                Genres = { new Genre { GenreID = 2, GenreTitle = "Test2" } }
            };

            // Act
            var result = await controller.Create(newBook);
            var editResult = await controller.Edit(1, editBook);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(editResult);
            Assert.Equal("Index", redirect.ActionName);
            var saved = await context.Books.FirstAsync();
            Assert.Equal("Test Title 2", saved.BookTitle);
        }

        [Fact]
        public async Task DeleteBook_WhenValid()
        {
            // Arrange
            var context = CreateDbContext();
            BookController controller = CreateController(context);

            Book newBook = new Book
            {
                BookID = 1,
                BookTitle = "Test Title 1",
                BookDescription = "Test Description 1",
                Genres = { new Genre { GenreID = 1, GenreTitle = "Test" } }

            };

            // Act
            var result = await controller.Create(newBook);
            var deleteResult = await controller.DeleteBook(1);
            var listResult = controller.List(1);

            // Assert
            var view = Assert.IsType<ViewResult>(listResult);
            var model = Assert.IsType<BooksListViewModels>(view.Model);

            Assert.Empty(model.Books);
        }
    }
}
