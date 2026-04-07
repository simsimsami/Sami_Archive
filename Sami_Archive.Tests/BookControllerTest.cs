using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using Sami_Archive.Controllers;
using Sami_Archive.Models;
using Sami_Archive.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using static Sami_Archive.Tests.TestData.SharedTestData;
using static Sami_Archive.Tests.GenreControllerTest;
using static Sami_Archive.Tests.AuthorControllerTest;

namespace Sami_Archive.Tests
{
    public class BookControllerTest
    {
        public static BookController CreateBookController(StoreDbContext context)
        {
            var repo = new EFBookRepository(context);
            return new BookController(context, repo);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange the mock data
            var mock = TestData.SharedTestData.BookMockRepo();

            // Arrange the controller and page size
            BookController controller = new BookController(null, mock.Object) { PageSize = 2 };

            // Act - declare a view model
            var result = controller.Index(1);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<BooksListViewModels>(viewResult.Model);


            // Assert the pagination view model
            PagingInfo pageInfo = model.PagingInfo;
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
            var mock = TestData.SharedTestData.BookMockRepo();

            BookController controller = new BookController(null, mock.Object) { PageSize = 3 };

            // Act - no filters, looking at the second page
            var result = controller.Index(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<BooksListViewModels>(viewResult.Model);

            //Assert
            Book[] bookArray = model.Books.ToArray();
            Assert.True(bookArray.Length == 3);
            Assert.Equal("B1", bookArray[0].BookTitle);
            Assert.Equal("B2", bookArray[1].BookTitle);
            Assert.NotEqual("B3", bookArray[1].BookTitle);
        }

        [Fact]
        public void Can_Access_Repository()
        {
            // Arrange - mock data
            var mock = TestData.SharedTestData.BookMockRepo();

            BookController controller = new BookController(null, mock.Object) { PageSize = 3 };

            // Act - getting access to the repo
            var result = controller.Index(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<BooksListViewModels>(viewResult.Model);


            // Assert - checking if the controller can access the bookRepository
            Book[] bookArray = model.Books.ToArray();
            Assert.Equal(3, bookArray.Length);
            Assert.NotNull(bookArray);
        }
        
        [Fact]
        // NOT FINISHED
        public void Can_Filter_Books()
        {
            // Arrange - setup mock repo
            var mock = TestData.SharedTestData.BookMockRepo();

            // Arrange - setup controller
            BookController controller = new BookController(null, mock.Object) { PageSize = 3 };

            // Filtering books by genre and or author.
        }

        [Fact]
        public async Task CreateBook_WhenValid()
        {
            // Arrange
            var context = CreateDbContext();

            BookController Bcontroller = CreateBookController(context);
            GenreController Gcontroller = CreateGenreController(context);
            AuthorController Acontroller = CreateAuthorController(context);

            List<KeyValuePair<long, string>> Genres = new List<KeyValuePair<long, string>>();
            List<KeyValuePair<long, string>> Authors = new List<KeyValuePair<long, string>>();

            Genres.Add(new KeyValuePair<long, string>(1, "G1"));
            Genres.Add(new KeyValuePair<long, string>(2, "G2"));

            Authors.Add(new KeyValuePair<long, string>(1, "A1"));
            Authors.Add(new KeyValuePair<long, string>(2, "A2"));

            List<long> SelectG = new List<long>();
            List<long> SelectA = new List<long>();

            SelectG.Add(1);
            SelectA.Add(1);


            var vm = new CreateBookViewModel
            {
                BookTitle = "Test Title",
                BookDescription = "Test Description",
                Genres = Genres,
                Authors = Authors,
                SelectedGenres = SelectG,
                SelectedAuthors = SelectA,
            };

            // Act
            var result = await Bcontroller.Create(vm);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            var saved = await context.Books
                .Include(b => b.Genres)
                .Include(b => b.Authors)
                .FirstOrDefaultAsync();

            Assert.Equal("Test Title", saved.BookTitle);


            Assert.NotEmpty(saved.Genres);

            return;
        }

        [Fact]
        public async Task UpdateBook_WhenValid()
        {
            // Arrange
            var context = TestData.SharedTestData.CreateDbContext();
            BookController controller = CreateBookController(context);

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
            //var result = await controller.Create(newBook);
            //var editResult = await controller.Edit(1, editBook);

            //// Assert
            //var redirect = Assert.IsType<RedirectToActionResult>(editResult);
            //Assert.Equal("Index", redirect.ActionName);
            //var saved = await context.Books.FirstAsync();
            //Assert.Equal("Test Title 2", saved.BookTitle);

            return;
        }

        [Fact]
        public async Task DeleteBook_WhenValid()
        {
            // Arrange
            var context = TestData.SharedTestData.CreateDbContext();
            BookController controller = CreateBookController(context);

            Book newBook = new Book
            {
                BookID = 1,
                BookTitle = "Test Title 1",
                BookDescription = "Test Description 1",
                Genres = { new Genre { GenreID = 1, GenreTitle = "Test" } }

            };

            //// Act
            //var create = await controller.Create(newBook);
            //var delete = await controller.DeleteBook(1);
            //var viewResult = controller.Index(1);

            //// Assert
            //var view = Assert.IsType<ViewResult>(viewResult);
            //var model = Assert.IsType<BooksListViewModels>(view.Model);

            //Assert.Empty(model.Books);

            //return;
        }
    }
}
