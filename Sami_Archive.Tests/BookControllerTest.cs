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
    public class BookControllerTest
    {
        private StoreDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
                .Options;
            return new StoreDbContext( options );
        }

        [Fact]
        public async Task CreateBook_WhenValid()
        {
            // Arrange
            var context = CreateDbContext();
            BookController controller = new BookController( context );

            Book newBook = new Book
            {
                BookID = 1,
                Title = "Test Title",
                Description = "Test Description",
                Genre = "Test Genre"
            };

            // Act
            var result = await controller.Create(newBook);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            var saved = await context.Books.FirstAsync();
            Assert.Equal("Test Title", saved.Title);
        }

        [Fact]
        public async Task UpdateBook_WhenValid()
        {
            // Arrange
            var context = CreateDbContext();
            BookController controller = new BookController( context );

            Book newBook = new Book
            {
                BookID = 1,
                Title = "Test Title 1",
                Description = "Test Description 1",
                Genre = "Test Genre 1"
            };

            Book editBook = new Book
            {
                BookID = 1,
                Title = "Test Title 2",
                Description = "Test Description 2",
                Genre = "Test Genre 2"
            };

            // Act
            var result = await controller.Create(newBook);
            var editResult = await controller.Edit(1, editBook);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(editResult);
            Assert.Equal("Index", redirect.ActionName);
            var saved = await context.Books.FirstAsync();
            Assert.Equal("Test Title 2", saved.Title);
        }

        [Fact]
        public async Task CreateBook_WhenInvalid()
        {
            // Arrange
            var context = CreateDbContext();
            BookController controller = new BookController( context );

            controller.ModelState.AddModelError("Title", "Required");


        }
    }
}
