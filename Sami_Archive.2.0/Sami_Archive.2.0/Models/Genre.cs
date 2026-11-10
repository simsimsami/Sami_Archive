using System.ComponentModel.DataAnnotations;

namespace Sami_Archive._2._0.Models
{
    public class Genre
    {
        public long GenreID { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(100)]
        [Required]
        public string GenreName { get; set; } = String.Empty;
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
