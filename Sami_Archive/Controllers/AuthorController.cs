using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Sami_Archive.Models;
using Sami_Archive.Models.ViewModels;
using System.Net;
using System.Net.Http;

namespace Sami_Archive.Controllers
{
    public class AuthorController : Controller
    {
        public int PageSize = 10;
        private readonly StoreDbContext _context;
        private IAuthorRepository authorRepository;

        public AuthorController(StoreDbContext context, IAuthorRepository repo)
        {
            _context = context;
            authorRepository = repo;
        }

        [HttpGet]
        public ViewResult Index(int page = 1, string? nameFilter = null)
        {
            // I want to present the list of author names, conditional if there is a nameFilter
            var query = authorRepository.Authors
                .Where(a => nameFilter == null || a.AuthorName == nameFilter);

            var totalItems = query.Count();

            var authors = query
                .OrderBy(a => a.AuthorID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            return View(new AuthorsListViewModels
            {
                Authors = authors,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = totalItems
                },
                NameFilter = nameFilter
            });
        }

        [HttpGet]
        public ViewResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await authorRepository.AddAuthorAsync(author);
                    return RedirectToAction("Index", "Author");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            return View(author);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long AuthorID)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorID == AuthorID);
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long AuthorID, [Bind("AuthorID,AuthorName")] Author author)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await authorRepository.UpdateAuthorAsync(author);
                    return RedirectToAction("Index", "Author");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            ModelState.AddModelError("", "Error in editing author");
            return RedirectToAction("Index", "Book");

        }

        [HttpGet]
        public async Task<IActionResult> DeleteForm(long AuthorID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var author = await _context.Authors.FindAsync(AuthorID);
                    if (author == null) { return NotFound(); }
                    ;
                    await authorRepository.DeleteAuthorAsync(AuthorID);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            ModelState.AddModelError("", "Error deleting author");
            return RedirectToAction("Index", "Author");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAuthor(long AuthorID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var author = await _context.Authors.FindAsync(AuthorID);
                    if (author == null) return NotFound($"Book with ID = {AuthorID} not found");
                    
                    await authorRepository.DeleteAuthorAsync(AuthorID);
                    return RedirectToAction("Index", "Author");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            ModelState.AddModelError("", "Error deleting author");
            return RedirectToAction("Index", "Author");

        }
    }
}
