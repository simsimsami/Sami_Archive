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
using Microsoft.AspNetCore.Authorization;

namespace Sami_Archive.Tests
{
    public class AuthorControllerTest
    {
        public static Mock<IAuthorRepository> CreateMockRepo()
        {
            var mock = new Mock<IAuthorRepository>();

            mock.Setup(m => m.Authors).Returns((new List<Author>
            {
                new Author { AuthorID = 1, AuthorName = "AN1", Books = { new Book { BookID = 1, BookTitle = "B1", BookDescription = "D1", } } },
                new Author { AuthorID = 2, AuthorName = "AN2", Books = { new Book { BookID = 2, BookTitle = "B2", BookDescription = "D2", } } },
                new Author { AuthorID = 3, AuthorName = "AN3", Books = { new Book { BookID = 3, BookTitle = "B3", BookDescription = "D3", } } },
                new Author { AuthorID = 4, AuthorName = "AN4", Books = { new Book { BookID = 4, BookTitle = "B4", BookDescription = "D4", } } },
                new Author { AuthorID = 5, AuthorName = "AN5", Books = { new Book { BookID = 5, BookTitle = "B5", BookDescription = "D5", } } },
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
        private AuthorController CreateController(StoreDbContext context)
        {
            var repo = new EFAuthorRepository(context);
            return new AuthorController(context, repo);
        }

        [Fact]
        public void Can_Send_View_Model()
        {
            // Arrange
            var mock = AuthorControllerTest.CreateMockRepo();

            // Arrange the controller and page size
            AuthorController controller = new AuthorController(null, mock.Object) { PageSize = 2};

            AuthorsListViewModels view = controller.Index(1)?.ViewData.Model as AuthorsListViewModels ?? new();

            Assert.Equal(3, view.PagingInfo.TotalPages);
            Assert.NotNull(view);
            Assert.Equal(5, view.PagingInfo.TotalItems);
        }

        [Fact]
        public void Can_Paginate()
        {
            // Arrange
            var mock = AuthorControllerTest.CreateMockRepo();

            AuthorController controller = new AuthorController(null, mock.Object) { PageSize = 3};
            AuthorsListViewModels results = controller.Index(2)?.ViewData.Model as AuthorsListViewModels ?? new();

            Author[] authorArray = results.Authors.ToArray();
            Assert.True(authorArray.Length == 2);
            Assert.Equal(4, authorArray[0].AuthorID);
            Assert.NotEqual(1, authorArray[1].AuthorID);
        }

        [Fact]
        public void Can_FilterAuthors()
        {
            var mock = AuthorControllerTest.CreateMockRepo();

            AuthorController controller = new AuthorController(null, mock.Object) { PageSize = 3 };
            AuthorsListViewModels result = controller.Index(1, "AN1")?.ViewData.Model as AuthorsListViewModels ?? new();

            Author[] authorArray = result.Authors.ToArray();
            Assert.Equal("AN1", authorArray[0].AuthorName);
            Assert.NotEqual("AN2", authorArray[0].AuthorName);
            Assert.NotEmpty(authorArray);
        }

        [Fact]
        public async Task CreateAuthor_WhenValid()
        {
            var context = CreateDbContext();
            AuthorController controller = CreateController(context);

            Author authArray = new Author
            {
                AuthorID = 1,
                AuthorName = "Test Author",
            };

            var result = await controller.Create(authArray);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            var saved = await context.Authors.FirstAsync();
            Assert.Equal("Test Author", saved.AuthorName);
        }

        [Fact]
        public async Task UpdateAuthor_WhenValid()
        {
            var context = CreateDbContext();
            var controller = CreateController(context);

            Author author = new Author
            {
                AuthorID = 1,
                AuthorName = "Test Author",
            };

            Author editAuthor = new Author
            {
                AuthorID = 1,
                AuthorName = "Test Author edit"
            };

            var result = await controller.Create(author);
            var editResult = await controller.Edit(1, editAuthor);

            var redirect = Assert.IsType<RedirectToActionResult>(editResult);
            Assert.Equal("Index", redirect.ActionName);

            var saved = await context.Authors.FirstAsync();
            Assert.Equal("Test Author edit", saved.AuthorName);
            Assert.Equal(1, saved.AuthorID);
        }

        [Fact]
        public async Task DeleteAuthor_WhenValid()
        {
            var context = CreateDbContext();
            var controller = CreateController(context);

            Author author = new Author
            {
                AuthorID = 1,
                AuthorName = "Test Author"
            };

            var result = controller.Create(author);
            var delete = controller.DeleteAuthor(1);
            var viewResult = controller.Index(1);

            var view = Assert.IsType<ViewResult>(viewResult);
            var model = Assert.IsType<AuthorsListViewModels>(view.Model);

            Assert.Empty(model.Authors);
        }
    }
}
