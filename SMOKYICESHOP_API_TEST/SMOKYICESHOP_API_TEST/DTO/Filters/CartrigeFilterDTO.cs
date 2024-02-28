namespace SMOKYICESHOP_API_TEST.DTO.Filters
{
    public class CartrigeFilterDTO
    {
        public string? Search { get; set; }
        public short? MinPrice { get; set; }
        public short? MaxPrice { get; set; }
        public bool? SortToCheap { get; set; }
        public IEnumerable<string>? ProducerNames { get; set; }
        public IEnumerable<string>? SpiralTypes { get; set; }
        public IEnumerable<double>? CartrigeCapacities { get; set; }
        public IEnumerable<double>? Resistances { get; set; }
    }
}
