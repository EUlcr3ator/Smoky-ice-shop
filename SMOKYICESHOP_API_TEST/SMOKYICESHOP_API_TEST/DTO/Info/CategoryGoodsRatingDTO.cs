using SMOKYICESHOP_API_TEST.DTO.Goods;
using System.Collections;

namespace SMOKYICESHOP_API_TEST.DTO.Info
{
    public class CategoryGoodsRatingDTO
    {
        public string CategoryName { get; set; }
        public int CountInOrders { get; set; }
        public IEnumerable<RatingGoodDto> Goods { get; set; }

        public CategoryGoodsRatingDTO()
        {
            CategoryName = String.Empty;
            CountInOrders = 0;
        }
    }
}
