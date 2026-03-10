namespace Sami_Archive.Models.ViewModels
{
    public class BooksListViewModels
    {
        public IEnumerable<Book> Books { get; set; } = Enumerable.Empty<Book>();
        public PagingInfo PagingInfo { get; set; } = new();
        public string? GenreFilter { get; set; }
        public string? TitleFilter {  get; set; }
        public string? AuthorFilter { get; set; }
    }
}
