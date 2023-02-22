namespace CommandService.Data.DTOs.Command
{
    public class UpdateCommandDTO
    {
        public int Id { get; set; }
        public string Describtion { get; set; }
        public string CommandLine { get; set; }
        public int PlatformId { get; set; }
    }
}