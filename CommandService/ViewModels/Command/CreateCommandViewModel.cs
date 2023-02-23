using System.ComponentModel.DataAnnotations;

namespace CommandService.ViewModels.Command
{
    public class CreateCommandViewModel
    {
        public string Describtion { get; set; }

        [Required]
        public string CommandLine { get; set; }
    }
}