using Sami_Archive._2._0.Models;

namespace Sami_Archive._2._0.Data
{
    public class BookAuthor
    {
        public long BookID { get; set; }
        public long AuthorID { get; set; }

        public Book Book { get; set; } = null!;
        public Author Author { get; set; } = null!;
    }
}
