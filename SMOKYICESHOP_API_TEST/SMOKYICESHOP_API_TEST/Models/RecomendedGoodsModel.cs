using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DtoService;
using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class RecomendedGoodsModel
    {
        private readonly SmokyIceDbContext _dbcontext;
        private readonly DefaultDtoService _defaultDtoService;

        public RecomendedGoodsModel(SmokyIceDbContext dbcontext, DefaultDtoService defaultDtoService)
        {
            _dbcontext = dbcontext;
            _defaultDtoService = defaultDtoService;
        }

        public IEnumerable<DefaultGoodDTO> GetAllRecomendedGoods()
        {
            return _dbcontext.RecomendedGoods
                .Include(g => g.Good)
                    .ThenInclude(p => p.DiscountGood)
                .Select(x => _defaultDtoService.CreateDefaultGood(x.Good))
                .ToList();
        }

        public void RemoveFromRecomended(Guid goodId)
        {
            RecomendedGood recomendedGood = new RecomendedGood() { GoodId = goodId };
            _dbcontext.RecomendedGoods.Attach(recomendedGood);
            _dbcontext.RecomendedGoods.Remove(recomendedGood);
            _dbcontext.SaveChanges();
        }

        public void AddToRecomendedGood(Guid goodId)
        {
            RecomendedGood recomendedGood = new RecomendedGood() { GoodId = goodId };
            _dbcontext.RecomendedGoods.Add(recomendedGood);
            _dbcontext.SaveChanges();
        }
    }
}
