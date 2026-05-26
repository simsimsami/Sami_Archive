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
using Sami_Archive.Tests.TestData;

namespace Sami_Archive.Tests
{
    public class BookControllerTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public BookControllerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public async Task<StoreDbContext> PopulateDatabase()
        {
            var context = CreateDbContext();

            var bookMock = BookMockRepo();
            var genreMock = GenreMockRepo();
            var authorMock = AuthorMockRepo();

            context.AddRange(bookMock.Object.Books);
            context.AddRange(genreMock.Object.Genres);
            context.AddRange(authorMock.Object.Authors);

            await context.SaveChangesAsync();

            return context;
        }
        public static BookController CreateBookController(StoreDbContext context)
        {
            var bookRepo = new EFBookRepository(context);
            var genreRepo = new EFGenreRepository(context);
            var authorRepo = new EFAuthorRepository(context);
            return new BookController(context, bookRepo, genreRepo, authorRepo) { PageSize = 3 };
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange the mock data
            var bookMock = TestData.SharedTestData.BookMockRepo();
            var authorMock = TestData.SharedTestData.AuthorMockRepo();
            var genreMock = TestData.SharedTestData.GenreMockRepo();

            // Arrange the controller
            BookController controller = new BookController(null, bookMock.Object, genreMock.Object, authorMock.Object);

            // Act - declare a view model
            var result = controller.Index(1);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<BooksListViewModels>(viewResult.Model);


            // Assert the pagination view model
            PagingInfo pageInfo = model.PagingInfo;
            Assert.NotNull(pageInfo);
            Assert.Equal(1, pageInfo.CurrentPage);
            Assert.NotEqual(2, pageInfo.ItemsPerPage);
            Assert.Equal(10, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(1, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Paginate()
        {
            // Arrange - Declaring the object mock - giving it mock data.
            var bookMock = TestData.SharedTestData.BookMockRepo();
            var authorMock = TestData.SharedTestData.AuthorMockRepo();
            var genreMock = TestData.SharedTestData.GenreMockRepo();

            BookController controller = new BookController(null, bookMock.Object, genreMock.Object, authorMock.Object);

            // Act - no filters, looking at the second page
            var result = controller.Index(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<BooksListViewModels>(viewResult.Model);

            //Assert
            Book[] bookArray = model.Books.ToArray();
            Assert.True(bookArray.Length == 5);
        }

        [Fact]
        public void Can_Access_Repository()
        {
            // Arrange - mock data
            var bookMock = TestData.SharedTestData.BookMockRepo();
            var authorMock = TestData.SharedTestData.AuthorMockRepo();
            var genreMock = TestData.SharedTestData.GenreMockRepo();

            BookController controller = new BookController(null, bookMock.Object, genreMock.Object, authorMock.Object);

            // Act - getting access to the repo
            var result = controller.Index(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<BooksListViewModels>(viewResult.Model);


            // Assert - checking if the controller can access the bookRepository
            Book[] bookArray = model.Books.ToArray();
            Assert.Equal(5, bookArray.Length);
            Assert.NotNull(bookArray);
        }

        [Fact]
        public async Task Can_Filter_Books()
        {
            // Get the context, get the mocks, 
            var context = await PopulateDatabase();

            // Arrange - setup mock repo
            var mock = TestData.SharedTestData.BookMockRepo();

            // Arrange - setup controller
            BookController controller = CreateBookController(context);

            // Filtering books by genre and or author.

            // setup keys
            List<KeyValuePair<long, string>> Genres = new List<KeyValuePair<long, string>>();
            List<KeyValuePair<long, string>> Authors = new List<KeyValuePair<long, string>>();

            Genres.Add(new KeyValuePair<long, string>(1, "G1"));
            Genres.Add(new KeyValuePair<long, string>(2, "G2"));
            Genres.Add(new KeyValuePair<long, string>(3, "G3"));
            Authors.Add(new KeyValuePair<long, string>(1, "AN1"));
            Authors.Add(new KeyValuePair<long, string>(2, "AN2"));

            var SelectG = new List<long>();
            var SelectA = new List<long>();

            SelectG.Add(1);
            SelectG.Add(2);
            SelectA.Add(1);

            var vm1 = new CreateBookViewModel
            {
                BookTitle = "Test Title 1",
                BookDescription = "Test Description 1",
                Genres = Genres,
                Authors = Authors,
                SelectedGenres = SelectG,
                SelectedAuthors = SelectA,
            };

            SelectG.Add(3);
            SelectA.Add(2);
            var vm2 = new CreateBookViewModel
            {
                BookTitle = "Test Title 2",
                BookDescription = "Test Description 2",
                Genres = Genres,
                Authors = Authors,
                SelectedGenres = SelectG,
                SelectedAuthors = SelectA,
            };

            await controller.Create(vm1);
            await controller.Create(vm2);

            var genreFilter = new List<string> { "G3" };
            var authorFilter = new List<string> { "AN2" };

            var result = controller.Index(1, null, genreFilter, authorFilter);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<BooksListViewModels>(viewResult.Model);

            Book[] bookArray = model.Books.ToArray();

            //Assert.Single(bookArray);
            Assert.All(bookArray, b =>
            {
                var genreCheck = b.Genres.Select(g => g.GenreTitle);
                Assert.Contains("G3", genreCheck);

                var authorCheck = b.Authors.Select(a => a.AuthorName);
                Assert.Contains("AN2", authorCheck);
            });

            // The current code filters, rather then getting a list of similar items. Eg, G3 comes up, but also G1
            // I either want to work on making it so that specific things come up, or keep it as filtering
        }

        [Fact]
        public async Task GetBook()
        {
            var mock = BookMockRepo();

            var context = CreateDbContext();

            var controller = CreateBookController(context);

            context.AddRange(mock.Object.Books);

            context.SaveChanges();

            var bookCount = context.Books.Count();

            Assert.True(bookCount > 3);
        }

        [Fact]
        public async Task CreateBook_WhenValid()
        {
            // Arrange - setting up mocks, keyvaluepairs and createbookviewmodel
            var context = await PopulateDatabase();

            BookController Bcontroller = CreateBookController(context);

            List<KeyValuePair<long, string>> Genres = new List<KeyValuePair<long, string>>();
            List<KeyValuePair<long, string>> Authors = new List<KeyValuePair<long, string>>();

            Genres.Add(new KeyValuePair<long, string>(1, "G1"));
            Genres.Add(new KeyValuePair<long, string>(2, "G2"));

            Authors.Add(new KeyValuePair<long, string>(1, "A1"));
            Authors.Add(new KeyValuePair<long, string>(2, "A2"));

            List<long> SelectG = new List<long>();
            List<long> SelectA = new List<long>();

            SelectG.Add(1);
            SelectG.Add(2);
            SelectA.Add(1);
            SelectA.Add(2);

            // Act - populate context in memory, put the vm in the index.Create().

            var vm = new CreateBookViewModel
            {
                BookTitle = "Test Title",
                BookDescription = "Test Description",
                Genres = Genres,
                Authors = Authors,
                SelectedGenres = SelectG,
                SelectedAuthors = SelectA,
            };

            var result = await Bcontroller.Create(vm);

            // Assert - checking if Test Title has genre and author in their collections

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            var saved = context.Books.Last();
            Assert.NotEmpty(saved.Genres);
            Assert.NotEmpty(saved.Authors);

        }

        [Fact]
        public async Task UpdateBook_WhenValid()
        {
            // Get the context, get the mocks, 

            var context = await PopulateDatabase();

            // Get bookcontroller

            BookController controller = CreateBookController(context);

            // setup keys

            List<KeyValuePair<long, string>> Genres = new List<KeyValuePair<long, string>>();
            List<KeyValuePair<long, string>> Authors = new List<KeyValuePair<long, string>>();

            Genres.Add(new KeyValuePair<long, string>(1, "G1"));
            Genres.Add(new KeyValuePair<long, string>(2, "G2"));
            Genres.Add(new KeyValuePair<long, string>(3, "G3"));
            Authors.Add(new KeyValuePair<long, string>(1, "A1"));
            Authors.Add(new KeyValuePair<long, string>(2, "A2"));

            var SelectG = new List<long>();
            var SelectA = new List<long>();

            SelectG.Add(1);
            SelectG.Add(2);
            SelectA.Add(1);
            SelectA.Add(2);

            var vm = new CreateBookViewModel
            {
                BookTitle = "Test Title",
                BookDescription = "Test Description",
                Genres = Genres,
                Authors = Authors,
                SelectedGenres = SelectG,
                SelectedAuthors = SelectA,
            };

            var result = await controller.Create(vm);
            var saved = await context.Books.LastAsync();
            var bookID = saved.BookID;
            SelectG.Add(3);

            var Editvm = new UpdateBooksViewModel
            {
                BookID = bookID,
                BookTitle = "Test Title 1",
                BookDescription = "Test Description 1",
                Genres = Genres,
                Authors = Authors,
                SelectedGenres = SelectG,
                SelectedAuthors = SelectA,
            };

            var EditResult = await controller.Edit(Editvm);
            var book = await context.Books.LastAsync();

            var genreCount = book.Genres.Count();

            Assert.Equal(3, genreCount);
        }

        [Fact]
        public async Task DeleteBook_WhenValid()
        {
            // creating context
            var context = await PopulateDatabase();

            BookController controller = CreateBookController(context);

            // create keys

            List<KeyValuePair<long, string>> Genres = new List<KeyValuePair<long, string>>();
            List<KeyValuePair<long, string>> Authors = new List<KeyValuePair<long, string>>();

            Genres.Add(new KeyValuePair<long, string>(1, "G1"));
            Authors.Add(new KeyValuePair<long, string>(1, "A1"));

            var SelectG = new List<long>();
            var SelectA = new List<long>();

            SelectG.Add(1);
            SelectA.Add(1);

            var vm = new CreateBookViewModel
            {
                BookTitle = "Test Title",
                BookDescription = "Test Description",
                Genres = Genres,
                Authors = Authors,
                SelectedGenres = SelectG,
                SelectedAuthors = SelectA
            };

            await controller.Create(vm);

            var saved = await context.Books.LastAsync();

            var count = await context.Books.CountAsync();

            var result = await controller.DeleteBook(saved.BookID);

            var newSaved = await context.Books.LastAsync();

            Assert.Empty(newSaved.Genres);
        }


        // I want to write invalid tests to improve my project

        [Fact]
        public async Task Invalid_Error_Method()
        {

        }
    }
}
