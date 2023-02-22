using System.ComponentModel.DataAnnotations;

namespace CommandService.ViewModels.Command
{
    public class UpdateCommandViewModel
    {
        [Required]
        public int Id { get; set; }

        public string Describtion { get; set; }

        [Required]
        public string CommandLine { get; set; }
    }
}