using Microsoft.AspNetCore.Identity;

namespace Sami_Archive._2._0.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<ListUser> ListUsers { get; set; } = new List<ListUser>();
        public ICollection<UserBookRating> UserBookRatings { get; set; } = new List<UserBookRating>();
        public ICollection<UserBookComment> UserBookComments { get; set; } = new List<UserBookComment>();
        public bool IsAdmin { get; set; } = false;
    }

}