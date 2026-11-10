using Sami_Archive._2._0.Models;

namespace Sami_Archive._2._0.Data
{
    public class UserBookComment
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;


        public long BookId { get; set; }
        public Book Book { get; set; } = null!;

        // Payload
        public string CommentText { get; set; } = null!;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
