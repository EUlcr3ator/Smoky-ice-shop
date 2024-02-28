namespace SMOKYICESHOP_API_TEST.DTO.Filters
{
    public class TobaccoFilterDTO
    {
        public string? Search { get; set; }
        public short? MinPrice { get; set; }
        public short? MaxPrice { get; set; }
        public bool? SortToCheap { get; set; }
        public byte? Sweet { get; set; }
        public byte? Sour { get; set; }
        public byte? Fresh { get; set; }
        public byte? Spicy { get; set; }
        public IEnumerable<string>? ProducerNames { get; set; }
        public IEnumerable<string>? Tastes { get; set; }
        public IEnumerable<string>? Strengths { get; set; }
        public IEnumerable<double>? Weights { get; set; }
    }
}
