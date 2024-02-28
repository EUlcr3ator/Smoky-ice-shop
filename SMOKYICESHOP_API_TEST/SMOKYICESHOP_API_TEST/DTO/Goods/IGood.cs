namespace SMOKYICESHOP_API_TEST.DTO.Goods
{
    public interface IGood
    {
        public Guid Id { get; set; }
        public Guid? ImageId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public short Price { get; set; }
        public short? DiscountPrice { get; set; }
        public bool IsSold { get; set; }
    }
}
