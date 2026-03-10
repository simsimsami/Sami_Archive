namespace Sami_Archive.Models
{
    public class Author
    {
        public long? AuthorID { get; set; }
        public string AuthorName { get; set; } = String.Empty;
        public ICollection<Book> Books { get; } = new List<Book>();
    }
}
