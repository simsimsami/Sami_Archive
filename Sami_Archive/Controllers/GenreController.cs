using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sami_Archive.Models;
using Sami_Archive.Models.ViewModels;
using System.Net;

namespace Sami_Archive.Controllers
{
    public class GenreController : Controller
    {
        public int PageSize = 4;
        private readonly StoreDbContext _context;
        private IGenreRepository genreRepository;

        public GenreController(StoreDbContext context, IGenreRepository repo)
        {
            _context = context;
            genreRepository = repo;
        }
        public ViewResult Index(int page = 1, string? title = null)
        {
            var query = genreRepository.Genres
                .Where(g => title == null || g.GenreTitle == title);

            var totalItems = query.Count();

            var genres = query
                .OrderBy(g => g.GenreTitle)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            return View(new GenresListViewModels
            {
                Genres = genres,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = totalItems
                },
                GenreFilter = title
            });
        }
        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Genre genre)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await genreRepository.AddGenreAsync(genre);
                    return RedirectToAction("Index", "Genre");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            ModelState.AddModelError("", "Unable to create genre");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long GenreID)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreID == GenreID);
            return View(genre);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long GenreID, [Bind("GenreID,GenreTitle")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await genreRepository.UpdateGenreAsync(genre);
                    return RedirectToAction("Index", "Genre");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            ModelState.AddModelError("", "Unable to edit genre");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteForm(long GenreID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await genreRepository.DeleteGenreAsync(GenreID);
                    return RedirectToAction("Index", "Genre");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            ModelState.AddModelError("","Error in delete genre");
            return RedirectToAction("Index", "Genre");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGenre(long GenreID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var genre = await _context.Genres.FindAsync(GenreID);

                    await genreRepository.DeleteGenreAsync(GenreID);
                    return RedirectToAction("Index", "Genre");
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
                }
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
