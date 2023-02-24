using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace CommandService.ViewModels.Command
{
    public class ReadCommandsViewModel
    {
        [Required]
        [Min(1)]
        public int PageSize { get; set; }

        [Required]
        [Min(1)]
        public int PageNumber { get; set; }

        public KeyValuePair<string, string>? SortInfo { get; set; }
        public string? DescribtionFilterValue { get; set; }
        public string? CommandLineFilterValue { get; set; }
    }
}