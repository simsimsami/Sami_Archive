using Sami_Archive._2._0.Models;

namespace Sami_Archive._2._0.Data
{
    public class ListUser
    {
        public long ListId { get; set; }
        public List List { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
    }
}
