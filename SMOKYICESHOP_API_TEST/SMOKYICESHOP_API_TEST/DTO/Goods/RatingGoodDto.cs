namespace SMOKYICESHOP_API_TEST.DTO.Goods
{
    public class RatingGoodDto
    {
        public DefaultGoodDTO GoodInfo { get; set; }
        public int CountInOrders { get; set; }

        public RatingGoodDto()
        {
            CountInOrders = 0;
        }
    }
}
