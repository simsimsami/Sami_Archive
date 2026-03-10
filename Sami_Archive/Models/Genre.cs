namespace Sami_Archive.Models
{
    public class Genre
    {
        public long GenreID { get; set; }
        public string GenreTitle { get; set; } = String.Empty;
        public ICollection<Book> Books { get; } = new List<Book>();
    }
}
