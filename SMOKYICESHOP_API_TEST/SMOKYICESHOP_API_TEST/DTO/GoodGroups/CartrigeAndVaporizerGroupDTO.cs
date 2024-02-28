using SMOKYICESHOP_API_TEST.DTO.FieldValues;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DTO.Info;

namespace SMOKYICESHOP_API_TEST.DTO.GoodGroups
{
    public class CartrigeAndVaporizerGroupDTO : IGroupDTO
    {
        public Guid GroupId { get; set; }
        public double? CartrigeCapacity { get; set; }
        public string? SpiralType { get; set; } = null!;
        public bool IsVaporizer { get; set; }
        public string? Line { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;


        public Guid ImageId { get; set; }
        public ProducerDTO Producer { get; set; } = null!;

        public IEnumerable<CartrigeAndVaporizersGoodDTO> Goods { get; set; } = null!;
        public IEnumerable<ICustomGrouping<double, Guid>> Resistances { get; set; } = null!;

        public IEnumerable<ShortGroupDTO> Pods { get; set; } = null!;
    }
}
