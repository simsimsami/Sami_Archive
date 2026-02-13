using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Sdk;
using Sami_Archive.Models;
using Sami_Archive.Controllers;
using Sami_Archive.Models.ViewModels;

namespace Sami_Archive.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange the mock data
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns((new Book[]
            {
                new Book { BookID = 1, Title = "B1", Description = "D1", Genre = "G1" },
                new Book { BookID = 2, Title = "B2", Description = "D2", Genre = "G2" },
                new Book { BookID = 3, Title = "B3", Description = "D3", Genre = "G3" },
                new Book { BookID = 4, Title = "B4", Description = "D4", Genre = "G4" },
                new Book { BookID = 5, Title = "B5", Description = "D5", Genre = "G5" },
            }).AsQueryable<Book>());

            // Arrange the controller and page size
            HomeController controller = new HomeController(mock.Object) { PageSize = 2};

            // Act - declare a view model
            BooksListViewModels result = controller.Index(null, 1)?.ViewData.Model as BooksListViewModels ?? new();

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
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns((new Book[]
            {
                new Book { BookID = 1, Title = "B1", Description = "D1", Genre = "G1" },
                new Book { BookID = 2, Title = "B2", Description = "D2", Genre = "G2" },
                new Book { BookID = 3, Title = "B3", Description = "D3", Genre = "G3" },
                new Book { BookID = 4, Title = "B4", Description = "D4", Genre = "G4" },
                new Book { BookID = 5, Title = "B5", Description = "D5", Genre = "G5" },
            }).AsQueryable<Book>());

            HomeController controller = new HomeController(mock.Object) { PageSize = 3 };

            // Act - no filters, looking at the second page
            BooksListViewModels result = controller.Index(null, 2, null)?.ViewData.Model as BooksListViewModels ?? new();

            Book[] bookArray = result.Books.ToArray();
            Assert.True(bookArray.Length == 2);
            Assert.Equal("B4", bookArray[0].Title);
            Assert.Equal("B5", bookArray[1].Title);
            Assert.NotEqual("B1", bookArray[1].Title);
        }

        [Fact]
        public void Can_Access_Repository()
        {
            // Arrange - mock data
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns((new Book[]
            {
                new Book { BookID = 1, Title = "B1", Description = "D1", Genre = "G1" },
                new Book { BookID = 2, Title = "B2", Description = "D2", Genre = "G2" },
                new Book { BookID = 3, Title = "B3", Description = "D3", Genre = "G3" },
                new Book { BookID = 4, Title = "B4", Description = "D4", Genre = "G4" },
                new Book { BookID = 5, Title = "B5", Description = "D5", Genre = "G5" },
            }).AsQueryable<Book>());

            HomeController controller = new HomeController(mock.Object) { PageSize = 3 };

            // Act - getting access to the repo
            BooksListViewModels result = controller.Index(null, 2, null)?.ViewData.Model as BooksListViewModels ?? new();

            // Assert - checking if the controller can access the repository
            Book[] bookArray = result.Books.ToArray();
            Assert.True(bookArray.Length == 2);
            Assert.NotNull(bookArray);

        }
    }
}
