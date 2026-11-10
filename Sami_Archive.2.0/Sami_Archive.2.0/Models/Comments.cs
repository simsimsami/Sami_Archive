using System.ComponentModel.DataAnnotations;

namespace Sami_Archive._2._0.Models
{
    public class Comments
    {
        public long CommentID { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(500)]
        [Required]
        public string Comment { get; set; } = String.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
