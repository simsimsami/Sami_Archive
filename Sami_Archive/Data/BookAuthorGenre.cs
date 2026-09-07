using Sami_Archive.Models;

namespace Sami_Archive.Data
{
    public class BookAuthorGenre
    {
        // Composite Primary Key / Foreign Keys
        public int BookID { get; set; }
        public int AuthorID { get; set; }
        public int GenreID { get; set; }

        // Navigation properties
        public Book Book { get; set; } = null!;
        public Author Author { get; set; } = null!;
        public Genre Genre { get; set; } = null!;
    }
}
