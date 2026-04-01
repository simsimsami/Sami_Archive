using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sami_Archive.Models.ViewModels
{
    public static class SelectListFactory
    {
        public static List<SelectListItem> FromPairs(IEnumerable<(string value, string text)> items) =>
            items.Select(i =>
        new SelectListItem
        {
            Value = i.value,
            Text = i.value
        }).ToList();
    }
}
