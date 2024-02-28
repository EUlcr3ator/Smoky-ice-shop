using SMOKYICESHOP_API_TEST.DTO.Info;

namespace SMOKYICESHOP_API_TEST.DTO.GoodGroups
{
    public class ShortGroupDTO : IGroupDTO
    {
        public Guid GroupId { get; set; }
        public string? Line { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;

        public Guid ImageId { get; set; }
        public ProducerDTO Producer { get; set; } = null!;

        public short Price { get; set; }
        public short? DiscountPrice { get; set; }
    }
}
