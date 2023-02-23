namespace CommandService.Data.DTOs.Command
{
    public class CreateCommandDTO
    {
        public string Describtion { get; set; }
        public string CommandLine { get; set; }
        public int PlatformId { get; set; }
    }
}