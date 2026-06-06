using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sami_Archive.Models.ViewModels
{
    [Bind(Exclude = "Genres, Authors, BookID")]
    public class UpdateBooksViewModel
    {
        [Required]
        public long BookID { get; set; }
        [StringLength(100)]
        [Required]
        public string BookTitle { get; set; } = String.Empty;
        [StringLength(500)]
        [Required]
        public string BookDescription { get; set; } = String.Empty;
        public required List<KeyValuePair<long, string>> Genres { get; set; } = new();
        public required List<KeyValuePair<long, string>> Authors { get; set; } = new();
        [Required]
        public List<long> SelectedGenres { get; set; } = new();
        [Required]
        public List<long> SelectedAuthors { get; set; } = new();
    }
}
