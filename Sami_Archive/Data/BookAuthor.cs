using Sami_Archive.Models;

namespace Sami_Archive.Data
{
    public class BookAuthor
    {
        public long BookID { get; set; }
        public long AuthorID { get; set; }

        public Book Book { get; set; } = null!;
        public Author Author { get; set; } = null!;
    }
}
