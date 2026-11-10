using Sami_Archive._2._0.Models;

namespace Sami_Archive._2._0.Data
{
    public class BookGenre
    {
        public long BookID { get; set; }
        public long GenreID { get; set; }

        public Book Book { get; set; } = null!;
        public Genre Genre { get; set; } = null!;
    }
}
