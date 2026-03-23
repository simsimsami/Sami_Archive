using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sami_Archive.Models;
using Sami_Archive.Models.ViewModels;

namespace Sami_Archive.Controllers
{
    public class AuthorController : Controller
    {
        public int PageSize = 4;
        private readonly StoreDbContext _context;
        private IAuthorRepository authorRepository;

        public AuthorController(StoreDbContext context,  IAuthorRepository repo)
        {
            _context = context;
            authorRepository = repo;
        }

        public ViewResult Index(int page = 1, string? nameFilter = null)
        {
            // I want to present the list of author names, conditional if there is a nameFilter
            var query = authorRepository.Authors
                .Where(a => nameFilter == null || a.AuthorName == nameFilter);

            var totalItems = query.Count();

            var authors = query
                .OrderBy(a => a.AuthorID)
                .Skip((page - 1) * PageSize)
                .Take(totalItems);

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
    }
}
