using SMOKYICESHOP_API_TEST.DTO.GoodGroups;

namespace SMOKYICESHOP_API_TEST.DTO.Goods
{
    public class GoodsCollection
    {
        public short MaxPrice { get; set; }
        public short MinPrice { get; set; }
        public int TotalPages { get; set; }
        public bool IsLastPage { get; set; }

        public IEnumerable<DefaultGoodDTO> Goods { get; set; } = null!;
    }
}
