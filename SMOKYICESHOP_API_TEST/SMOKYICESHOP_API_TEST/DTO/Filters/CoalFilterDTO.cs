namespace SMOKYICESHOP_API_TEST.DTO.Filters
{
    public class CoalFilterDTO
    {
        public string? Search { get; set; }
        public short? MinPrice { get; set; }
        public short? MaxPrice { get; set; }
        public bool? SortToCheap { get; set; }
        public IEnumerable<string>? ProducerNames { get; set; }
        public IEnumerable<string>? Types { get; set; }
        public IEnumerable<double>? Weights { get; set; }
    }
}
