namespace Sami_Archive.Models.ViewModels
{
    public class GenresListViewModels
    {
        public IEnumerable<Genre> Genres { get; set; } = Enumerable.Empty<Genre>();
        public PagingInfo PagingInfo { get; set; } = new();
        public string? GenreFilter { get; set; }


    }
}
