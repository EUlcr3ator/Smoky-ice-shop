namespace SMOKYICESHOP_API_TEST.DTO.Filters
{
    public class LiquidFilterDTO
    {
        public string? Search { get; set; }
        public short? MinPrice { get; set; }
        public short? MaxPrice { get; set; }
        public bool? SortToCheap { get; set; }
        public IEnumerable<string>? ProducerNames { get; set; }
        public IEnumerable<byte>? NicotineStrengths { get; set; }
        public IEnumerable<byte>? Capacities { get; set; }
        public IEnumerable<string>? TasteGroups { get; set; }
        public IEnumerable<string>? Tastes { get; set; }
    }
}
