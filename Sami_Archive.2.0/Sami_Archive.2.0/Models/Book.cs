using Sami_Archive._2._0.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sami_Archive._2._0.Models
{
    public class Book
    {
        public long BookID { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(100)]
        [Required]
        public string BookTitle { get; set; } = String.Empty;
        public string BookDescription { get; set; } = String.Empty;
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public ICollection<Author> Authors { get; set; } = new List<Author>();
        public ICollection<UserBookRating> Ratings { get; set; } = new List<UserBookRating>();
        public ICollection<UserBookComment> Comments { get; set; } = new List<UserBookComment>();
        public ICollection<ListingBook> ListingBooks { get; set; } = new List<ListingBook>();
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
