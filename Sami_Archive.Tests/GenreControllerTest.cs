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
using static Sami_Archive.Tests.TestData.SharedTestData;


namespace Sami_Archive.Tests
{
    public class GenreControllerTest
    {
        public static GenreController CreateGenreController(StoreDbContext context)
        {
            var repo = new EFGenreRepository(context);
            return new GenreController(context, repo);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange mock data
            var mock = TestData.SharedTestData.GenreMockRepo();

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
            var mock = TestData.SharedTestData.GenreMockRepo();

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
            var mock = TestData.SharedTestData.GenreMockRepo();

            GenreController controller = new GenreController(null, mock.Object) { PageSize = 3 };

            // Act - getting access to the repo
            GenresListViewModels result = controller.Index(1)?.ViewData.Model as GenresListViewModels;

            // Assert - checking if the controller can access the bookRepository

            Genre[] genreArray = result.Genres.ToArray();

            Assert.Equal(3, genreArray.Length);
            Assert.NotNull(genreArray);
        }
        
        [Fact]
        public async Task CreateGenre_WhenValid()
        {
            // Arrange
            var context = TestData.SharedTestData.CreateDbContext();
            GenreController controller = CreateGenreController(context);

            Genre newGenre = new Genre
            {
                GenreID = 1,
                GenreTitle = "Test Genre 1",
            };

            // Act
            var result = await controller.Create(newGenre);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            var saved = await context.Genres.FirstAsync();
            Assert.Equal("Test Genre 1", saved.GenreTitle);
        }

        [Fact]
        public async Task UpdateGenre_WhenValid()
        {
            // Arrange
            var context = TestData.SharedTestData.CreateDbContext();
            GenreController controller = CreateGenreController(context);

            Genre genre = new Genre
            {
                GenreID = 1,
                GenreTitle = "Test Genre 1"
            };

            Genre newGenre = new Genre
            {
                GenreID = 1,
                GenreTitle = "Test Genre 2"
            };

            // Act
            var create = await controller.Create(genre);
            var result = await controller.Edit(1, newGenre);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            var saved = await context.Genres.FirstAsync();
            Assert.Equal("Test Genre 2", saved.GenreTitle);
        }

        [Fact]
        public async Task DeleteGenre_WhenValid()
        {
            // Arrange
            var context = TestData.SharedTestData.CreateDbContext();
            var controller = CreateGenreController(context);
            
            Genre genre = new Genre
            {
                GenreID = 1,
                GenreTitle = "Test Genre 1"
            };

            // Act
            var create = await controller.Create(genre);
            var delete = await controller.DeleteGenre(1);
            var viewResult = controller.Index(1);

            // Assert
            var view = Assert.IsType<ViewResult>(viewResult);
            var model = Assert.IsType<GenresListViewModels>(view.Model);
            
            Assert.Empty(model.Genres);
        }
    }
}
