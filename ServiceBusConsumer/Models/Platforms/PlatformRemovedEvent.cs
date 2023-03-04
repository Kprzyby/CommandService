using System.ComponentModel.DataAnnotations;

namespace ServiceBusConsumer.Models.Platforms
{
    internal class PlatformRemovedEvent : Event
    {
        [Required]
        public int PlatformId { get; set; }
    }
}