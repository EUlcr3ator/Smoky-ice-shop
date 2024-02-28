using SMOKYICESHOP_API_TEST.DTO.FieldValues;

namespace SMOKYICESHOP_API_TEST.DTO.Goods
{
    public class EcigaretteGoodDTO : IGood
    {
        public Guid Id { get; set; }
        public Guid? ImageId { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public short Price { get; set; }
        public short? DiscountPrice { get; set; }
        public bool IsSold { get; set; }

        public byte Sweet { get; set; }
        public byte Sour { get; set; }
        public byte Fresh { get; set; }
        public byte Spicy { get; set; }
        public string Taste { get; set; } = null!;
    }
}
