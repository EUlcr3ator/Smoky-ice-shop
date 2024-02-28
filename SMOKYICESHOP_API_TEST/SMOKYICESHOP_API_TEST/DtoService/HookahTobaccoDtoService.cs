using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Services;

namespace SMOKYICESHOP_API_TEST.DtoService
{
    public class HookahTobaccoDtoService : BaseDtoService
    {
        private readonly DiscountsService _discountsService;

        public HookahTobaccoDtoService(DiscountsService discountsService)
        {
            _discountsService = discountsService;
        }

        public HookahTobaccoGroupDTO CreateGroupDto(HookahTobaccoGroup group)
        {
            ArgumentNullException.ThrowIfNull(group, nameof(group));
            ArgumentNullException.ThrowIfNull(group.Producer, nameof(group.Producer));

            HookahTobaccoGroupDTO item = new HookahTobaccoGroupDTO
            {
                GroupId = group.Id,
                Sweet = group.Sweet,
                Sour = group.Sour,
                Spicy = group.Spicy,
                Fresh = group.Fresh,
                Strength = group.Strength.Name,
                Taste = group.TasteMix.Name,
                Line = group.Line,
                Name = group.Name,
                Category = CategoryEnum.HookahTobacco.ToString(),
                ImageId = group.ImageId,
                Producer = CreateProducer(group.Producer)
            };

            item.Goods = group.HookahTobaccos.Select(x => CreateGoodDto(x));

            item.Weights = group.HookahTobaccos
                .GroupBy(x => x.Weight)
                .Select(x => CreateIdGroup(x));

            return item;
        }

        public HookahTobaccoGoodDTO CreateGoodDto(HookahTobacco entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            ArgumentNullException.ThrowIfNull(entity.Good, nameof(entity.Good));
            ArgumentNullException.ThrowIfNull(entity.Weight, nameof(entity.Weight));

            HookahTobaccoGoodDTO dto = new();

            short? discount = _discountsService.GetDiscountPrice(entity.Good.DiscountGood);
            base.FillGoodDto(dto, entity.Good, discount);
            dto.Weight = entity.Weight;

            return dto;
        }

        public ShortGroupDTO CreateShortGroupDto(HookahTobaccoGroup groupEntity)
        {
            ArgumentNullException.ThrowIfNull(groupEntity, nameof(groupEntity));
            ArgumentNullException.ThrowIfNull(groupEntity.Producer, nameof(groupEntity.Producer));

            ArgumentNullException.ThrowIfNull(groupEntity.HookahTobaccos.First().Good, "Good");

            ShortGroupDTO groupDTO = BaseCreateShortGroup(groupEntity, CategoryEnum.HookahTobacco);

            groupDTO.Producer = CreateProducer(groupEntity.Producer);

            Good good = groupEntity.HookahTobaccos.Select(x => x.Good).MinBy(x => x.Price);

            groupDTO.Price = good.Price;
            groupDTO.DiscountPrice = _discountsService.GetDiscountPrice(good.DiscountGood);

            return groupDTO;
        }

        public HookahTobacco CreateGoodEntity(CreateHookahTobaccoGoodDTO dto)
        {
            return new HookahTobacco
            {
                Good = base.CreateGoodEntity(dto, CategoryEnum.HookahTobacco),
                GroupId = dto.GroupId,
                Weight = dto.Weight
            };
        }

        public HookahTobaccoGroup CreateGroupEntity(CreateHookahTobaccoGroupDTO dto, Guid tasteMixId, byte strengthId)
        {
            return new HookahTobaccoGroup
            {
                Sweet = dto.Sweet,
                Sour = dto.Sour,
                Fresh = dto.Fresh,
                Spicy = dto.Spicy,
                StrengthId = strengthId,
                Line = dto.Line,
                Name = dto.Name,
                ImageId = dto.ImageId,
                TasteMixId = tasteMixId,
                ProducerId = dto.ProducerId
            };
        }

        public IQueryable<HookahTobaccoGroup> IncludeForShortGroup(IQueryable<HookahTobaccoGroup> groups)
        {
            return groups
                .Include(x => x.Strength)
                .Include(x => x.Producer)
                .Include(x => x.HookahTobaccos)
                    .ThenInclude(g => g.Good)
                        .ThenInclude(x => x.DiscountGood);
        }

        public IQueryable<HookahTobaccoGroup> IncludeForGroup(IQueryable<HookahTobaccoGroup> groups)
        {
            return groups
                .Include(x => x.TasteMix)
                .Include(x => x.Strength)
                .Include(x => x.Producer)
                .Include(x => x.HookahTobaccos)
                    .ThenInclude(g => g.Good)
                        .ThenInclude(x => x.DiscountGood);
        }

        public IQueryable<HookahTobacco> IncludeForGood(IQueryable<HookahTobacco> goods)
        {
            return goods
                .Include(x => x.Group)
                    .ThenInclude(x => x.Producer)
                .Include(x => x.Good)
                    .ThenInclude(x => x.DiscountGood);
        }
    }
}
