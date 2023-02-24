namespace CommandService.Data.DTOs.Command
{
    public class ReadCommandsResponseDTO
    {
        public IEnumerable<ReadCommandDTO> Commands { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public KeyValuePair<string, string>? SortInfo { get; set; }
        public string? DescribtionFilterValue { get; set; }
        public string? CommandLineFilterValue { get; set; }
    }
}