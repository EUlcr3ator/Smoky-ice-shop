using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Services
{
    public class DiscountsService
    {
        private readonly SmokyIceDbContext _dbcontext;

        public DiscountsService(SmokyIceDbContext context)
        {
            _dbcontext = context;
        }

        public short? GetDiscountPrice(Guid goodId)
        {
            DiscountGood? discount = _dbcontext.DiscountGoods.FirstOrDefault(x => x.GoodId == goodId);
            return GetDiscountPrice(discount);
        }

        public short? GetDiscountPrice(DiscountGood? discountGood)
        {
            short? discount = null;

            if (discountGood != null)
                if (IsValidDiscount(discountGood))
                    discount = discountGood.DiscountPrice;

            return discount;
        }

        private bool IsValidDiscount(DiscountGood discountGood)
        {
            DateTime dateToCheck = DateTime.Now;

            if (dateToCheck >= discountGood.StartDate || discountGood.StartDate == null)
            {
                if (dateToCheck < discountGood.EndDate || discountGood.EndDate == null)
                {
                    return true;
                }
                else
                {
                    RemoveDiscount(discountGood.GoodId);
                }
            }

            return false;
        }

        private void RemoveDiscount(Guid goodId)
        {
            DiscountGood removeDiscount = _dbcontext.DiscountGoods
                .First(x => x.GoodId == goodId);

            _dbcontext.DiscountGoods.Remove(removeDiscount);
        }
    }
}
