namespace SMOKYICESHOP_API_TEST.DTO.Filters
{
    public class PodFilterDTO
    {
        public string? Search { get; set; }
        public short? MinPrice { get; set; }
        public short? MaxPrice { get; set; }
        public bool? SortToCheap { get; set; }
        public IEnumerable<string>? ProducerNames { get; set; }
        public IEnumerable<string>? Materials { get; set; }
        public IEnumerable<double>? EvaporatorResistances { get; set; }
        public IEnumerable<short>? Battareys { get; set; }
        public IEnumerable<byte>? CartrigeCapacities { get; set; }
    }
}
