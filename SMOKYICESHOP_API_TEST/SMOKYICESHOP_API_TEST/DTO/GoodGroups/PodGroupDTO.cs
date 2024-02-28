using SMOKYICESHOP_API_TEST.DTO.FieldValues;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DTO.Info;

namespace SMOKYICESHOP_API_TEST.DTO.GoodGroups
{
    public class PodGroupDTO : IGroupDTO
    {
        public Guid GroupId { get; set; }
        public double Weight { get; set; }
        public string Material { get; set; } = null!;
        public short Battarey { get; set; }
        public double CartrigeCapacity { get; set; }
        public double EvaporatorResistance { get; set; }
        public string Power { get; set; } = null!;
        public string Port { get; set; } = null!;
        public string? Line { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;

        public Guid ImageId { get; set; }
        public ProducerDTO Producer { get; set; } = null!;

        public IEnumerable<PodGoodDTO> Goods { get; set; } = null!;
        public IEnumerable<ICustomGrouping<string, Guid>> Appearances { get; set; } = null!;

        public IEnumerable<ShortGroupDTO> Cartriges { get; set; } = null!;
    }
}
