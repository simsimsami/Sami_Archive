namespace Sami_Archive.Models
{
    public interface IAuthorRepository
    {
        IQueryable<Author> Authors { get; }
        Task AddAuthorAsync(Author author);
        Task UpdateAuthorAsync(Author author);
        Task DeleteAuthorAsync(Author author);
    }
}
