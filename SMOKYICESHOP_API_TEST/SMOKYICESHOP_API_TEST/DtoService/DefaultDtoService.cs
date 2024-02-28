using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;
using System.Text.RegularExpressions;

namespace SMOKYICESHOP_API_TEST.DtoService
{
    public class DefaultDtoService : BaseDtoService
    {
        private readonly DiscountsService _discountsService;
        private readonly SmokyIceDbContext _dbcontext;

        public DefaultDtoService(DiscountsService discountsService, SmokyIceDbContext dbcontext)
        {
            _discountsService = discountsService;
            _dbcontext = dbcontext;
        }

        public DefaultGoodDTO CreateDefaultGood(Good entity)
        {
            short? discount = _discountsService.GetDiscountPrice(entity.DiscountGood);
            Guid groupId = GetGroupId(entity);
            return CreateDefaultGood(entity, discount, groupId);
        }

        public SearchGoodDTO CreateSearchGood(Good entity)
        {
            DefaultGoodDTO dto = CreateDefaultGood(entity);
            return new SearchGoodDTO
            {
                Id = dto.Id,
                ImageId = dto.ImageId,
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                DiscountPrice = dto.DiscountPrice,
                IsSold = dto.IsSold,
                GroupId = dto.GroupId
            };
        }

        public Guid GetGroupId(Good entity)
        {
            Guid groupId = Guid.Empty;
            CategoryEnum category = (CategoryEnum)entity.CategoryId;

            switch (category)
            {
                case CategoryEnum.CartrigesAndVaporizers:
                    groupId = _dbcontext.CartrigesAndVaporizers.First(x => x.GoodId == entity.Id).GroupId;
                    break;
                case CategoryEnum.Coals:
                    groupId = _dbcontext.Coals.First(x => x.GoodId == entity.Id).GroupId;
                    break;
                case CategoryEnum.ECigarettes:
                    groupId = _dbcontext.Ecigarettes.First(x => x.GoodId == entity.Id).GroupId;
                    break;
                case CategoryEnum.HookahTobacco:
                    groupId = _dbcontext.HookahTobaccos.First(x => x.GoodId == entity.Id).GroupId;
                    break;
                case CategoryEnum.Liquids:
                    groupId = _dbcontext.Liquids.First(x => x.GoodId == entity.Id).GroupId;
                    break;
                case CategoryEnum.Pods:
                    groupId = _dbcontext.Pods.First(x => x.GoodId == entity.Id).GroupId;
                    break;
                default:
                    throw new NotImplementedException("Category not Found");
            }

            return groupId;
        }
    }
}
