namespace Sami_Archive.Models.ViewModels
{
    public class BooksListViewModels
    {
        public IEnumerable<Book> Books { get; set; } = Enumerable.Empty<Book>();
        public IEnumerable<Genre> Genres { get; set; } = Enumerable.Empty<Genre>();
        public PagingInfo PagingInfo { get; set; } = new();
        public List<string>? AuthorFilter { get; set; }
        public List<string>? GenreFilter { get; set; }
        public string? TitleFilter {  get; set; }
    }
}
