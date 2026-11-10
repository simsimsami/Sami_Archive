using Sami_Archive.Models;

namespace Sami_Archive.Data
{
    public class BookGenre
    {
        public long BookID { get; set; }
        public long GenreID { get; set; }

        public Book Book { get; set; } = null!;
        public Genre Genre { get; set; } = null!;
    }
}
