using SMOKYICESHOP_API_TEST.DTO.FieldValues;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DTO.Info;

namespace SMOKYICESHOP_API_TEST.DTO.GoodGroups
{
    public class EcigaretteGroupDTO : IGroupDTO
    {
        public Guid GroupId { get; set; }
        public byte EvaporatorVolume { get; set; }
        public short BattareyCapacity { get; set; }
        public int PuffsCount { get; set; }
        public string? Line { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;

        public Guid ImageId { get; set; }
        public ProducerDTO Producer { get; set; } = null!;

        public IEnumerable<EcigaretteGoodDTO> Goods { get; set; } = null!;
        public IEnumerable<ICustomGrouping<string, Guid>> Tastes { get; set; } = null!;
    }
}
