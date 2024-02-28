using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Services;

namespace SMOKYICESHOP_API_TEST.DtoService
{
    public class CoalDtoService : BaseDtoService
    {
        private readonly DiscountsService _discountsService;

        public CoalDtoService(DiscountsService discountsService)
        {
            _discountsService = discountsService;
        }

        public CoalGroupDTO CreateGroupDto(CoalsGroup group)
        {
            ArgumentNullException.ThrowIfNull(group, nameof(group));
            ArgumentNullException.ThrowIfNull(group.Producer, nameof(group.Producer));
            ArgumentNullException.ThrowIfNull(group.Coals, nameof(group.Coals));

            CoalGroupDTO item = new CoalGroupDTO
            {
                GroupId = group.Id,
                Type = group.Type,
                CubeSize = group.CubeSize,
                Line = group.Line,
                Name = group.Name,
                Category = CategoryEnum.Coals.ToString(),
                ImageId = group.ImageId,
                Producer = CreateProducer(group.Producer)
            };

            item.Goods = group.Coals.Select(x => CreateGoodDto(x));

            item.Weights = group.Coals
                .GroupBy(x => x.Weight)
                .Select(x => CreateIdGroup(x));

            return item;
        }

        public CoalGoodDTO CreateGoodDto(Coal entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            ArgumentNullException.ThrowIfNull(entity.Good, nameof(entity.Good));
            ArgumentNullException.ThrowIfNull(entity.Weight, nameof(entity.Weight));

            CoalGoodDTO dto = new();

            short? discount = _discountsService.GetDiscountPrice(entity.Good.DiscountGood);
            base.FillGoodDto(dto, entity.Good, discount);
            dto.Weight = entity.Weight;

            return dto;
        }

        public ShortGroupDTO CreateShortGroupDto(CoalsGroup groupEntity)
        {
            ArgumentNullException.ThrowIfNull(groupEntity, nameof(groupEntity));
            ArgumentNullException.ThrowIfNull(groupEntity.Producer, nameof(groupEntity.Producer));
            ArgumentNullException.ThrowIfNull(groupEntity.Coals, nameof(groupEntity.Coals));

            ArgumentNullException.ThrowIfNull(groupEntity.Coals.First().Good, "Good");

            ShortGroupDTO groupDTO = BaseCreateShortGroup(groupEntity, CategoryEnum.Coals);

            groupDTO.Producer = CreateProducer(groupEntity.Producer);

            Good good = groupEntity.Coals.Select(x => x.Good).MinBy(x => x.Price);

            groupDTO.Price = good.Price;
            groupDTO.DiscountPrice = _discountsService.GetDiscountPrice(good.DiscountGood);

            return groupDTO;
        }

        public Coal CreateGoodEntity(CreateCoalGoodDTO dto)
        {
            return new Coal
            {
                Good = base.CreateGoodEntity(dto, CategoryEnum.Coals),
                GroupId = dto.GroupId,
                Weight = dto.Weight
            };
        }

        public CoalsGroup CreateGroupEntity(CreateCoalGroupDTO dto)
        {
            return new CoalsGroup
            {
                Type = dto.Type,
                CubeSize = dto.CubeSize,
                Line = dto.Line,
                Name = dto.Name,
                ImageId = dto.ImageId,
                ProducerId = dto.ProducerId
            };
        }

        public IQueryable<CoalsGroup> IncludeForShortGroup(IQueryable<CoalsGroup> groups)
        {
            return groups
                .Include(x => x.Producer)
                .Include(x => x.Coals)
                    .ThenInclude(g => g.Good)
                        .ThenInclude(x => x.DiscountGood);
        }

        public IQueryable<CoalsGroup> IncludeForGroup(IQueryable<CoalsGroup> groups)
        {
            return groups
                .Include(x => x.Producer)
                .Include(x => x.Coals)
                    .ThenInclude(g => g.Good)
                        .ThenInclude(x => x.DiscountGood);
        }

        public IQueryable<Coal> IncludeForGood(IQueryable<Coal> goods)
        {
            return goods
                .Include(x => x.Group)
                    .ThenInclude(x => x.Producer)
                .Include(x => x.Good)
                    .ThenInclude(x => x.DiscountGood);
        }
    }
}
