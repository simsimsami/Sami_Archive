using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sami_Archive._2._0.Models
{
    public class Author
    {
        public long AuthorID { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(100)]
        [Required]
        public string AuthorName { get; set; } = String.Empty;
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
