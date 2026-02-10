namespace Sami_Archive.Models
{
    public interface IBookRepository
    {
        IQueryable<Book> Books { get; }
    }
}
