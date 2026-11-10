using Sami_Archive._2._0.Models;

namespace Sami_Archive._2._0.Data
{
    public class UserBookRating
    {
        // get book and rating
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public long BookId { get; set; }
        public Book Book { get; set; } = null!;

        // Payload
        public int RatingValue { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    }
}
