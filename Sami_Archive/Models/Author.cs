using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sami_Archive.Models
{
    public class Author
    {
        public long AuthorID { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(20, MinimumLength = 3)]
        [Required]
        public string AuthorName { get; set; } = String.Empty;
        public ICollection<Book> Books { get; } = new List<Book>();
    }
}
