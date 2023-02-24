using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace CommandService.ViewModels.Platform
{
    public class ReadPlatformsViewModel
    {
        [Required]
        [Min(1)]
        public int PageSize { get; set; }

        [Required]
        [Min(1)]
        public int PageNumber { get; set; }

        public KeyValuePair<string, string>? SortInfo { get; set; }
        public string? NameFilterValue { get; set; }
    }
}