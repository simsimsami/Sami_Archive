using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc; // replace System.Web.Mvc

namespace Sami_Archive.Models.ViewModels
{
    public class UpdateBooksViewModel
    {
        [Required]
        public long BookID { get; set; }
        [StringLength(100)]
        [Required]
        public string BookTitle { get; set; } = string.Empty;
        [StringLength(500)]
        [Required]
        public string BookDescription { get; set; } = string.Empty;
        public required List<KeyValuePair<long, string>> Genres { get; set; } = new();
        public required List<KeyValuePair<long, string>> Authors { get; set; } = new();
        [Required]
        public List<long> SelectedGenres { get; set; } = new();
        [Required]
        public List<long> SelectedAuthors { get; set; } = new();
    }
}
