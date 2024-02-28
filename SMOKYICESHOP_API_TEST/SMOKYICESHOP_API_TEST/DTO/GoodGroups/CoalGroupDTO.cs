using SMOKYICESHOP_API_TEST.DTO.FieldValues;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DTO.Info;

namespace SMOKYICESHOP_API_TEST.DTO.GoodGroups
{
    public class CoalGroupDTO : IGroupDTO
    {
        public Guid GroupId { get; set; }
        public string Type { get; set; } = null!;
        public double CubeSize { get; set; }
        public string? Line { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;

        public Guid ImageId { get; set; }
        public ProducerDTO Producer { get; set; } = null!;

        public IEnumerable<CoalGoodDTO> Goods { get; set; } = null!;
        public IEnumerable<ICustomGrouping<double, Guid>> Weights { get; set; } = null!;
    }
}
