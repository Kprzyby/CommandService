using System.ComponentModel.DataAnnotations;

namespace CommandService.ViewModels.Platform
{
    public class CreatePlatformViewModel
    {
        [Required]
        public int ExternalId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}