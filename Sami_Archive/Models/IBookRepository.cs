namespace Sami_Archive.Models
{
    public interface IBookRepository
    {
        IQueryable<Book> Books { get; }
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(long id);
    }
}
