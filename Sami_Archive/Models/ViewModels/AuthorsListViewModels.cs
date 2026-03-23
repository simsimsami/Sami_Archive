namespace Sami_Archive.Models.ViewModels
{
    public class AuthorsListViewModels
    {
        public IEnumerable<Author> Authors { get; set; } = Enumerable.Empty<Author>();
        public PagingInfo PagingInfo { get; set; } = new();
        public string? NameFilter { get; set; }
    }
}
