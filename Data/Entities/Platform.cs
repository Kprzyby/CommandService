using System.ComponentModel.DataAnnotations;

namespace CommandService.Data.Entities
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ExternalId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}