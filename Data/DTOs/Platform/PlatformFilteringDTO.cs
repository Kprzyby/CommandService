namespace CommandService.Data.DTOs.Platform
{
    public class PlatformFilteringDTO
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public KeyValuePair<string, string>? SortInfo { get; set; }
        public string? NameFilterValue { get; set; }
    }
}