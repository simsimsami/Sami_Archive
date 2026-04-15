using Sami_Archive.Models.ViewModels;

namespace Sami_Archive.Models
{
    public interface IBookRepository
    {
        IQueryable<Book> Books { get; }
        Task AddBookAsync(CreateBookViewModel viewModel);
        Task UpdateBookAsync(UpdateBooksViewModel viewModel);
        Task DeleteBookAsync(long BookID);
    }
}
