using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommandService.Data.Entities
{
    public class Command
    {
        [Key]
        public int Id { get; set; }

        public string Describtion { get; set; }

        [Required]
        public string CommandLine { get; set; }

        [Required]
        public int PlatformId { get; set; }

        [ForeignKey("PlatformId")]
        public virtual Platform? Platform { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}