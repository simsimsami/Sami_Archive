using Sami_Archive.Models.ViewModels;

namespace Sami_Archive.Models
{
    public interface IBookRepository
    {
        IQueryable<Book> Books { get; }
        Task AddBookAsync(CreateBookViewModel book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(long BookID);
    }
}
