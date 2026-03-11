using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sami_Archive.Models;
using Sami_Archive.Models.ViewModels;

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
        public ViewResult Index(int genrePage = 1, string? title = null)
        {
            var query = genreRepository.Genres
                .Where(g => title == null || g.GenreTitle == title);

            var totalItems = query.Count();

            var genres = query
                .OrderBy(g => g.GenreTitle)
                .Skip((genrePage - 1) * PageSize)
                .Take(PageSize);

            return View(new GenresListViewModels
            {
                Genres = genres,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = genrePage,
                    ItemsPerPage = PageSize,
                    TotalItems = totalItems
                },
                GenreFilter = title
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long? GenreID)
        {
            if (GenreID == null)
            {
                return NotFound();
            }
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreID == GenreID);
            return View(genre);
        }
    }
}
