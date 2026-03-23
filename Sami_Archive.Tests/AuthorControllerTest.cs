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
        }
    }
}
