using SMOKYICESHOP_API_TEST.DTO.FieldValues;

namespace SMOKYICESHOP_API_TEST.DTO.Goods
{
    public class LiquidGoodDTO : IGood
    {
        public Guid Id { get; set; }
        public Guid? ImageId { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public short Price { get; set; }
        public short? DiscountPrice { get; set; }
        public bool IsSold { get; set; }

        public byte NicotineStrength { get; set; }
        public TasteMixDTO<TasteDTO> Taste { get; set; } = null!;
    }
}
