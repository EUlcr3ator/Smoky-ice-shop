using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DtoService;
using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class PopularGoodsModel
    {
        private readonly SmokyIceDbContext _dbcontext;
        private readonly DefaultDtoService _defaultDtoService;

        public PopularGoodsModel(SmokyIceDbContext dbcontext, DefaultDtoService defaultDtoService)
        {
            _dbcontext = dbcontext;
            _defaultDtoService = defaultDtoService;
        }

        public IEnumerable<DefaultGoodDTO> GetAllPopularGoods()
        {
            return _dbcontext.PopularGoods
                .Include(g => g.Good)
                    .ThenInclude(x => x.DiscountGood)
                .Select(x => _defaultDtoService.CreateDefaultGood(x.Good))
                .ToList();
        }

        public void RemoveFromPopular(Guid goodId)
        {
            PopularGood popularGood = new PopularGood() { GoodId = goodId };
            _dbcontext.PopularGoods.Attach(popularGood);
            _dbcontext.PopularGoods.Remove(popularGood);
            _dbcontext.SaveChanges();
        }

        public void AddToPopularGood(Guid goodId)
        {
            PopularGood popularGood = new PopularGood() { GoodId = goodId };
            _dbcontext.PopularGoods.Add(popularGood);
            _dbcontext.SaveChanges();
        }
    }
}
