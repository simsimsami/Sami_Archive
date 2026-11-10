using Sami_Archive._2._0.Models;

namespace Sami_Archive._2._0.Data
{
    public class ListingBook
    {
        // junction table for many-to-many relationship between Book and List, since multiple users will have their own lists, and a book can be in multiple lists

        public long BookId { get; set; }
        public Book Book { get; set; } = null!;

        public long ListId { get; set; }
        public List List { get; set; } = null!;
    }
}
