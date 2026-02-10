using System.ComponentModel.DataAnnotations.Schema;

namespace Sami_Archive.Models
{
    public class Book
    {
        public long? BookID { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string Genre { get; set; } = String.Empty;
    }
}
