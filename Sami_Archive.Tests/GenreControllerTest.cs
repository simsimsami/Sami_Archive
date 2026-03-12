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


namespace Sami_Archive.Tests
{
    public class GenreControllerTest
    {
        public static Mock<IGenreRepository> CreateMockRepo()
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
        private StoreDbContext CreateDBContext()
        {
            var options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
                .Options;
            return new StoreDbContext( options );
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange mock data
            var mock = GenreControllerTest.CreateMockRepo();

            // Arrange the controller and page size
            GenreController controller = new GenreController(null, mock.Object) { PageSize = 3 };

            // Act - declare a view model
            GenresListViewModels result = controller.Index(1)?.ViewData.Model as GenresListViewModels;

            // Assert the pagination view model
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.NotNull(pagingInfo);
            Assert.Equal(4, pagingInfo.TotalItems);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(1, pagingInfo.CurrentPage);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Paginate()
        {
            // Arrange mock data
            var mock = GenreControllerTest.CreateMockRepo();

            // Act - controller and result
            GenreController controller = new GenreController(null, mock.Object) { PageSize = 3 };

            GenresListViewModels result = controller.Index(2)?.ViewData.Model as GenresListViewModels;

            // Assert - checking paginate
            Genre[] genreArray = result.Genres.ToArray();
            Assert.Equal(2, result.PagingInfo.CurrentPage);
            Assert.NotEqual("G1", genreArray[0].GenreTitle);
            Assert.Equal("G4", genreArray[0].GenreTitle);
        }

        [Fact]
        public void Can_Access_Repository()
        {
            // Arrange mock data
            var mock = GenreControllerTest.CreateMockRepo();

            GenreController controller = new GenreController(null, mock.Object) { PageSize = 3 };

            // Act - getting access to the repo
            GenresListViewModels result = controller.Index(1)?.ViewData.Model as GenresListViewModels;

            // Assert - checking if the controller can access the bookRepository

            Genre[] genreArray = result.Genres.ToArray();

            Assert.Equal(3, genreArray.Length);
            Assert.NotNull(genreArray);
        }
    }
}
