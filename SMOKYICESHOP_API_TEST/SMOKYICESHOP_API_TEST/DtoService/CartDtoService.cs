using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DTO.Cart;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Services;

namespace SMOKYICESHOP_API_TEST.DtoService
{
    public class CartDtoService : BaseDtoService
    {
        private readonly DiscountsService _discountsService;
        private readonly DefaultDtoService _defaultDtoService;

        public CartDtoService(DiscountsService discountsService, DefaultDtoService defaultDtoService)
        {
            _discountsService = discountsService;
            _defaultDtoService = defaultDtoService;
        }

        public CartGoodDTO CreateGood(CartHasGood good)
        {
            CartGoodDTO cartGoodDto = new CartGoodDTO();
            short? discount = _discountsService.GetDiscountPrice(good.GoodId);

            FillGoodDto(cartGoodDto, good.Good, discount);
            cartGoodDto.Count = good.ProductCount;
            cartGoodDto.GroupId = _defaultDtoService.GetGroupId(good.Good);
            return cartGoodDto;
        }

        public IQueryable<CartHasGood> IncludeForGood(IQueryable<CartHasGood> goods)
        {
            return goods
                .Include(x => x.Good)
                    .ThenInclude(x => x.DiscountGood);
        }
    }
}
