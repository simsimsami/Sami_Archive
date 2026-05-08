using System.ComponentModel.DataAnnotations;

namespace Sami_Archive.Models
{
    public class Genre
    {
        public long GenreID { get; set; }
        [StringLength(100)]
        [Required]
        public string GenreTitle { get; set; } = String.Empty;
        public ICollection<Book> Books { get; } = new List<Book>();
    }
}
