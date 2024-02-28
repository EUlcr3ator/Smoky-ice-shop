using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;
using SMOKYICESHOP_API_TEST.DTO.FieldValues;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Services;
using System.Text;

namespace SMOKYICESHOP_API_TEST.DtoService
{
    public class LiquidDtoService : BaseDtoService
    {
        private readonly DiscountsService _discountsService;

        public LiquidDtoService(DiscountsService discountsService)
        {
            _discountsService = discountsService;
        }

        public LiquidGroupDTO CreateGroupDto(LiquidsGroup group)
        {
            ArgumentNullException.ThrowIfNull(group, nameof(group));
            ArgumentNullException.ThrowIfNull(group.Producer, nameof(group.Producer));
            ArgumentNullException.ThrowIfNull(group.Liquids, nameof(group.Liquids));

            LiquidGroupDTO item = new LiquidGroupDTO
            {
                GroupId = group.Id,
                NicotineType = group.NicotineType,
                Capacity = group.Capacity,
                Line = group.Line,
                Name = group.Name,
                Category = CategoryEnum.Liquids.ToString(),
                ImageId = group.ImageId,
                Producer = CreateProducer(group.Producer)
            };

            item.Goods = group.Liquids.Select(x => CreateGoodDto(x));

            
            item.Tastes = item.Goods
                .GroupBy(x => x.Taste.Name)
                .Select(x => new CustomGroupingDTO<string, Guid>
                {
                    Key = x.Key,
                    Ids = x.Select(y => y.Id)
                });

            item.Strength = item.Goods
                .Select(x => x.NicotineStrength)
                .Distinct()
                .Select(x => new CustomGroupingDTO<byte, string>
                {
                    Key = x,
                    Ids = item.Goods.Where(y => y.NicotineStrength == x)
                        .Select(y => y.Taste.Name)
                })
                .OrderBy(x => x.Key);

            return item;
            
        }

        public LiquidGoodDTO CreateGoodDto(Liquid entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            ArgumentNullException.ThrowIfNull(entity.Good, nameof(entity.Good));
            ArgumentNullException.ThrowIfNull(entity.Strength, nameof(entity.Strength));

            LiquidGoodDTO dto = new();

            short? discount = _discountsService.GetDiscountPrice(entity.Good.DiscountGood);
            base.FillGoodDto(dto, entity.Good, discount);
            dto.NicotineStrength = entity.Strength;
            dto.Taste = new TasteMixDTO<TasteDTO>
            {
                Id = entity.TasteMixId,
                Name = entity.TasteMix.Name,
                Tastes = entity.TasteMix.Tastes.Select(x => new TasteDTO
                {
                    Taste = x.Taste,
                    TasteGroup = x.TasteGroup
                })
            };

            return dto;
        }

        public ShortGroupDTO CreateShortGroupDto(LiquidsGroup groupEntity)
        {
            ArgumentNullException.ThrowIfNull(groupEntity, nameof(groupEntity));
            ArgumentNullException.ThrowIfNull(groupEntity.Producer, nameof(groupEntity.Producer));
            ArgumentNullException.ThrowIfNull(groupEntity.Liquids, nameof(groupEntity.Liquids));

            ArgumentNullException.ThrowIfNull(groupEntity.Liquids.First().Good, "Good");

            ShortGroupDTO groupDTO = BaseCreateShortGroup(groupEntity, CategoryEnum.Liquids);

            groupDTO.Producer = CreateProducer(groupEntity.Producer);

            Good good = groupEntity.Liquids.Select(x => x.Good).MinBy(x => x.Price);

            groupDTO.Price = good.Price;
            groupDTO.DiscountPrice = _discountsService.GetDiscountPrice(good.DiscountGood);

            return groupDTO;
        }

        public Liquid CreateGoodEntity(CreateLiquidGoodDTO dto, Guid tasteMixId)
        {
            return new Liquid
            {
                Good = base.CreateGoodEntity(dto, CategoryEnum.Liquids),
                GroupId = dto.GroupId,
                Strength = dto.NicotineStrength,
                TasteMixId = tasteMixId
            };
        }

        public LiquidsGroup CreateGroupEntity(CreateLiquidGroupDTO dto)
        {
            return new LiquidsGroup
            {
                NicotineType = dto.NicotineType,
                Capacity = dto.Capacity,
                Line = dto.Line,
                Name = dto.Name,
                ImageId = dto.ImageId,
                ProducerId = dto.ProducerId
            };
        }

        public IQueryable<LiquidsGroup> IncludeForShortGroup(IQueryable<LiquidsGroup> groups)
        {
            return groups
                .Include(x => x.Producer)
                .Include(x => x.Liquids)
                    .ThenInclude(g => g.Good)
                        .ThenInclude(x => x.DiscountGood);
        }

        public IQueryable<LiquidsGroup> IncludeForGroup(IQueryable<LiquidsGroup> groups)
        {
            return groups
                .Include(x => x.Producer)
                .Include(x => x.Liquids)
                    .ThenInclude(g => g.TasteMix)
                        .ThenInclude(x => x.Tastes)
                .Include(x => x.Liquids)
                    .ThenInclude(g => g.Good)
                        .ThenInclude(x => x.DiscountGood);
        }

        public IQueryable<Liquid> IncludeForGood(IQueryable<Liquid> goods)
        {
            return goods
                .Include(x => x.Group)
                    .ThenInclude(x => x.Producer)
                .Include(x => x.Good)
                    .ThenInclude(x => x.DiscountGood);
        }
    }
}
