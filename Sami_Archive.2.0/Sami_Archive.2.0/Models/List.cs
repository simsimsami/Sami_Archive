using Sami_Archive._2._0.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sami_Archive._2._0.Models
{
    public class List
    {
        public long ListID { get; set; }
        [StringLength(100)]
        [Required]
        public string ListName { get; set; } = String.Empty;
        public ICollection<ListUser> ListUser { get; set; } = new List<ListUser>();
        public ICollection<ListingBook> ListingBooks { get; set; } = new List<ListingBook>();
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
