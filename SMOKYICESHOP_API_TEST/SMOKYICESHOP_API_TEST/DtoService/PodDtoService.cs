using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Services;

namespace SMOKYICESHOP_API_TEST.DtoService
{
    public class PodDtoService : BaseDtoService
    {
        private readonly DiscountsService _discountsService;
        private readonly CartrigeDtoService _cartrigeService;

        public PodDtoService(DiscountsService discountsService)
        {
            _discountsService = discountsService;
            _cartrigeService = new(_discountsService);
        }

        public PodGroupDTO CreateGroupDto(PodsGroup group)
        {
            ArgumentNullException.ThrowIfNull(group, nameof(group));
            ArgumentNullException.ThrowIfNull(group.Producer, nameof(group.Producer));
            ArgumentNullException.ThrowIfNull(group.Cartriges, nameof(group.Cartriges));

            PodGroupDTO item = new PodGroupDTO
            {
                GroupId = group.Id,
                Weight = group.Weight,
                Material = group.Material,
                Battarey = group.Battarey,
                CartrigeCapacity = group.CartrigeCapacity,
                EvaporatorResistance = group.EvaporatorResistance,
                Power = group.Power,
                Port = group.Port,
                Line = group.Line,
                Name = group.Name,
                Category = CategoryEnum.Pods.ToString(),
                ImageId = group.ImageId,
                Producer = CreateProducer(group.Producer)
            };

            item.Goods = group.Pods.Select(x => CreateGoodDto(x));

            item.Appearances = group.Pods
                .GroupBy(x => x.Appearance)
                .Select(x => CreateIdGroup(x));

            item.Cartriges = group.Cartriges
                .Select(x => _cartrigeService.CreateShortGroupDto(x));

            return item;
        }

        public PodGoodDTO CreateGoodDto(Pod entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            ArgumentNullException.ThrowIfNull(entity.Good, nameof(entity.Good));
            ArgumentNullException.ThrowIfNull(entity.Appearance, nameof(entity.Appearance));

            PodGoodDTO dto = new();

            short? discount = _discountsService.GetDiscountPrice(entity.Good.DiscountGood);
            base.FillGoodDto(dto, entity.Good, discount);
            dto.Appearance = entity.Appearance;

            return dto;
        }

        public ShortGroupDTO CreateShortGroupDto(PodsGroup groupEntity)
        {
            ArgumentNullException.ThrowIfNull(groupEntity, nameof(groupEntity));
            ArgumentNullException.ThrowIfNull(groupEntity.Producer, nameof(groupEntity.Producer));

            ArgumentNullException.ThrowIfNull(groupEntity.Pods.First().Good, "Good");

            ShortGroupDTO groupDTO = BaseCreateShortGroup(groupEntity, CategoryEnum.Pods);

            groupDTO.Producer = CreateProducer(groupEntity.Producer);

            Good good = groupEntity.Pods.Select(x => x.Good).MinBy(x => x.Price);

            groupDTO.Price = good.Price;
            groupDTO.DiscountPrice = _discountsService.GetDiscountPrice(good.DiscountGood);

            return groupDTO;
        }

        public Pod CreateGoodEntity(CreatePodGoodDTO dto)
        {
            return new Pod
            {
                Good = base.CreateGoodEntity(dto, CategoryEnum.Pods),
                GroupId = dto.GroupId,
                Appearance = dto.Appearance
            };
        }

        public PodsGroup CreateGroupEntity(CreatePodGroupDTO dto)
        {
            return new PodsGroup
            {
                Weight = dto.Weight,
                Material = dto.Material,
                Battarey = dto.Battarey,
                CartrigeCapacity = dto.CartrigeCapacity,
                EvaporatorResistance = dto.EvaporatorResistance,
                Power = dto.Power,
                Port = dto.Port,
                Line = dto.Line,
                Name = dto.Name,
                ImageId = dto.ImageId,
                ProducerId = dto.ProducerId
            };
        }

        public IQueryable<PodsGroup> IncludeForShortGroup(IQueryable<PodsGroup> groups)
        {
            //!!! If changed, change IncludeForGroup in CartrigeDtoService !!!

            return groups
                .Include(x => x.Producer)
                .Include(x => x.Pods)
                    .ThenInclude(g => g.Good)
                        .ThenInclude(x => x.DiscountGood);

            //!!! If changed, change IncludeForGroup in CartrigeDtoService !!!
        }

        public IQueryable<PodsGroup> IncludeForGroup(IQueryable<PodsGroup> groups)
        {
            return groups
                .Include(x => x.Producer)
                .Include(x => x.Pods)
                    .ThenInclude(g => g.Good)
                        .ThenInclude(x => x.DiscountGood)

                .Include(x => x.Cartriges)
                    .ThenInclude(x => x.Producer)
                .Include(x => x.Cartriges)
                    .ThenInclude(x => x.CartrigesAndVaporizers)
                        .ThenInclude(g => g.Good)
                            .ThenInclude(x => x.DiscountGood);
        }

        public IQueryable<Pod> IncludeForGood(IQueryable<Pod> goods)
        {
            return goods
                .Include(x => x.Group)
                    .ThenInclude(x => x.Producer)
                .Include(x => x.Good)
                    .ThenInclude(x => x.DiscountGood);
        }
    }
}
