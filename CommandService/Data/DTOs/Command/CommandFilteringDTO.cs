namespace CommandService.Data.DTOs.Command
{
    public class CommandFilteringDTO
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public KeyValuePair<string, string>? SortInfo { get; set; }
        public string DescribtionFilterValue { get; set; }
        public string CommandLineFilterValue { get; set; }
    }
}