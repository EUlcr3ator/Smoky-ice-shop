using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DTO.Info;
using SMOKYICESHOP_API_TEST.DtoService;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Services;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class GoodsModel
    {
        private readonly DefaultDtoService _defaultDtoService;
        private readonly SearchService _searchService;
        private readonly SmokyIceDbContext _dbcontext;

        public GoodsModel(DefaultDtoService defaultDtoService, SearchService searchService, SmokyIceDbContext dbcontext)
        {
            _defaultDtoService = defaultDtoService;
            _searchService = searchService;
            _dbcontext = dbcontext;
        }

        public IEnumerable<DefaultGoodDTO> GetGoodsWithDiscount()
        {
            return _dbcontext.DiscountGoods
                .Include(x => x.Good)
                .Select(x => _defaultDtoService.CreateDefaultGood(x.Good))
                .ToList();
        }

        public IEnumerable<SearchGoodDTO> GetSimilar(Guid goodId)
        {
            return _searchService.GetSimilar(goodId);
        }

        public SearchCollection Search(int pageSize, int pageNumber, string text)
        {
            SearchCollection collection = new SearchCollection();
            collection.SearchText = text;
            collection.Goods = _searchService.Search(text);

            decimal pages = Decimal.Divide(collection.Goods.Count(), pageSize);
            pages = Decimal.Round(pages, MidpointRounding.ToPositiveInfinity);
            collection.TotalPages = collection.TotalPages = Decimal.ToInt32(pages);
            collection.IsLastPage = collection.TotalPages <= pageNumber;

            collection.Goods = collection.Goods
                .Skip(pageSize * pageNumber - pageSize)
                .Take(pageSize);

            return collection;
        }

        public DefaultGoodDTO GetGood(Guid goodId)
        {
            Good good = _dbcontext.Goods.First(x => x.Id == goodId);
            return _defaultDtoService.CreateDefaultGood(good);
        }

        public async Task<int> GetGoodsOrdersCount(Guid goodId, DateTime? dateFrom, DateTime? dateTo)
        {
            int count = 0;

            IQueryable<OrderHasGood> query = _dbcontext.OrderHasGoods
                .Include(x => x.Order)
                .Where(x => x.GoodId == goodId);

            if (dateFrom != null)
                query = query.Where(x => x.Order.CreationDate > dateFrom);

            if (dateTo != null)
                query = query.Where(x => x.Order.CreationDate < dateTo);

            await query
                .Select(x => x.ProductCount)
                .ForEachAsync(x => count += x);

            return count;
        }

        public IEnumerable<CategoryGoodsRatingDTO> GetGoodsRatings(DateTime? dateFrom, DateTime? dateTo)
        {
            IEnumerable<Good> goods = _dbcontext.Goods
                .Include(x => x.OrderHasGoods)
                    .ThenInclude(x => x.Order)
                .Where(x => x.OrderHasGoods.Count != 0)
                .ToList();

            List<RatingGoodDto> ratingGoods = new List<RatingGoodDto>();

            foreach (var item in goods)
            {
                IEnumerable<OrderHasGood> orderGoods = item.OrderHasGoods;

                if (dateFrom != null)
                    orderGoods = orderGoods.Where(x => x.Order.CreationDate > dateFrom);

                if (dateTo != null)
                    orderGoods = orderGoods.Where(x => x.Order.CreationDate < dateTo);

                int sumInOrders = orderGoods.Sum(x => x.ProductCount);

                ratingGoods.Add(new RatingGoodDto
                {
                    GoodInfo = _defaultDtoService.CreateDefaultGood(item),
                    CountInOrders = sumInOrders
                });
            }

            var groups = ratingGoods
                .OrderByDescending(x => x.CountInOrders)
                .GroupBy(x => x.GoodInfo.Category);

            foreach (var grouped in groups)
            {
                yield return new CategoryGoodsRatingDTO
                {
                    Goods = grouped,
                    CategoryName = grouped.Key,
                    CountInOrders = grouped.Sum(x => x.CountInOrders)
                };
            }
        }
    }
}
