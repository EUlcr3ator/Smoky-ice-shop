using SMOKYICESHOP_API_TEST.DTO.FieldValues;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DTO.Info;

namespace SMOKYICESHOP_API_TEST.DTO.GoodGroups
{
    public class HookahTobaccoGroupDTO : IGroupDTO
    {
        public Guid GroupId { get; set; }
        public byte Sweet { get; set; }
        public byte Sour { get; set; }
        public byte Fresh { get; set; }
        public byte Spicy { get; set; }
        public string Strength { get; set; } = null!;
        public string? Line { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;

        public Guid ImageId { get; set; }
        public ProducerDTO Producer { get; set; } = null!;
        public string Taste { get; set; } = null!;

        public IEnumerable<HookahTobaccoGoodDTO> Goods { get; set; } = null!;
        public IEnumerable<ICustomGrouping<double, Guid>> Weights { get; set; } = null!;
    }
}
