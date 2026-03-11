namespace Sami_Archive.Models
{
    public interface IGenreRepository
    {
        IQueryable<Genre> Genres { get; }
        Task AddGenreAsync(Genre genre);
        Task UpdateGenreAsync(Genre genre);
        Task DeleteGenreAsync(long id);
    }
}
