using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sami_Archive.Models.ViewModels
{
    public class CreateBookViewModel
    {
        public string BookTitle { get; set; } = String.Empty;
        public string BookDescription { get; set; } = String.Empty;
        public required List<KeyValuePair<long, string>> Genres { get; set; } = new();

        public required List<KeyValuePair<long, string>> Authors { get; set; } = new();

        public List<long> SelectedGenres { get; set; } = new();

        public List<long> SelectedAuthors { get; set; } = new();
    }
}
