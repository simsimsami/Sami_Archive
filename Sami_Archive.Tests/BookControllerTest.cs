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
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var returnedBook = Assert.IsType<Book>(created.Value);

            Assert.Equal("Test Title", returnedBook.Title);
            Assert.True(returnedBook.BookID > 0);
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
