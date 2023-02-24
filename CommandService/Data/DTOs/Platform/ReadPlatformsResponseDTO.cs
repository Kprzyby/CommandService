namespace CommandService.Data.DTOs.Platform
{
    public class ReadPlatformsResponseDTO
    {
        public IEnumerable<ReadPlatformDTO> Platforms { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public KeyValuePair<string, string>? SortInfo { get; set; }
        public string? NameFilterValue { get; set; }
    }
}