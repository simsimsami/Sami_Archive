using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Sami_Archive.Models.ViewModels
{
    public class CreateBookViewModel
    {
        [StringLength(100)]
        [Required]
        public string BookTitle { get; set; } = String.Empty;
        [StringLength(500)]
        [Required]
        public string BookDescription { get; set; } = String.Empty;

        [BindNever]
        public required List<KeyValuePair<long, string>> Genres { get; set; } = new();
        [BindNever]
        public required List<KeyValuePair<long, string>> Authors { get; set; } = new();
        
        
        public List<long> SelectedGenres { get; set; } = new();
        public List<long> SelectedAuthors { get; set; } = new();
    }
}
