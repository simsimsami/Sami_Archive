using System.ComponentModel.DataAnnotations.Schema;

namespace Sami_Archive.Models
{
    public class Book
    {
        public long BookID { get; set; }
        public string BookTitle { get; set; } = String.Empty;
        public string BookDescription { get; set; } = String.Empty;
        public ICollection<Genre> Genres { get; } = new List<Genre>();
        public ICollection<Author> Authors { get; } = new List<Author>();
    }
}
