using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DtoService;
using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Services
{
    public class SearchService
    {
        private readonly SmokyIceDbContext _dbcontext;
        private readonly DefaultDtoService _defaultDtoService;

        public SearchService(SmokyIceDbContext dbcontext, DefaultDtoService defaultDtoService)
        {
            _dbcontext = dbcontext;
            _defaultDtoService = defaultDtoService;
        }

        public IEnumerable<SearchGoodDTO> Search(string text)
        {
            IEnumerable<string> tags = text.Split(' ').Where(x => x != String.Empty);

            IEnumerable<SearchGoodDTO> goods = _dbcontext.Goods
                .OrderByDescending(x => x.CreationDate)
                .Select(x => _defaultDtoService.CreateSearchGood(x))
                .ToList();

            foreach (var item in goods)
                foreach (var tag in tags)
                    if (item.Name.ToLower().Contains(tag.ToLower()))
                        item.SearchPoints += tag.Length;

            return goods
                .Where(x => x.SearchPoints != 0)
                .OrderByDescending(x => x.SearchPoints);
        }

        public IEnumerable<SearchGoodDTO> GetSimilar(Guid goodId)
        {
            Good good = _dbcontext.Goods.First(x => x.Id == goodId);
            Guid groupId = _defaultDtoService.GetGroupId(good);

            return Search(good.Name)
                .Where(x => x.GroupId != groupId)
                .Take(10);
        }
    }
}
