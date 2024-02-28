using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;
using SMOKYICESHOP_API_TEST.DTO.FieldValues;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DTO.Info;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Services;
using System.Text.RegularExpressions;

namespace SMOKYICESHOP_API_TEST.DtoService
{
    public class CartrigeDtoService : BaseDtoService
    {
        private readonly DiscountsService _discountsService;

        public CartrigeDtoService(DiscountsService discountsService)
        {
            _discountsService = discountsService;
        }

        public CartrigeAndVaporizerGroupDTO CreateGroupDto(CartrigesAndVaporizersGroup group)
        {
            ArgumentNullException.ThrowIfNull(group, nameof(group));
            ArgumentNullException.ThrowIfNull(group.Producer, nameof(group.Producer));
            ArgumentNullException.ThrowIfNull(group.CartrigesAndVaporizers, nameof(group.CartrigesAndVaporizers));

            CartrigeAndVaporizerGroupDTO item = new CartrigeAndVaporizerGroupDTO
            {
                GroupId = group.Id,
                CartrigeCapacity = group.CartrigeCapacity,
                SpiralType = group.SpiralType,
                IsVaporizer = group.IsVaporizer,
                Line = group.Line,
                Name = group.Name,
                Category = CategoryEnum.CartrigesAndVaporizers.ToString(),
                ImageId = group.ImageId,
                Producer = CreateProducer(group.Producer)
            };

            item.Goods = group.CartrigesAndVaporizers.Select(x => CreateGoodDto(x));

            item.Resistances = group.CartrigesAndVaporizers
                .GroupBy(x => x.Resistance)
                .Select(x => CreateIdGroup(x));

            return item;
        }

        public CartrigeAndVaporizersGoodDTO CreateGoodDto(CartrigesAndVaporizer entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            ArgumentNullException.ThrowIfNull(entity.Good, nameof(entity.Good));
            ArgumentNullException.ThrowIfNull(entity.Resistance, nameof(entity.Resistance));

            CartrigeAndVaporizersGoodDTO dto = new();

            short? discount = _discountsService.GetDiscountPrice(entity.Good.DiscountGood);
            base.FillGoodDto(dto, entity.Good, discount);
            dto.Resistance = entity.Resistance;

            return dto;
        }

        public ShortGroupDTO CreateShortGroupDto(CartrigesAndVaporizersGroup groupEntity)
        {
            ArgumentNullException.ThrowIfNull(groupEntity, nameof(groupEntity));
            ArgumentNullException.ThrowIfNull(groupEntity.Producer, nameof(groupEntity.Producer));
            ArgumentNullException.ThrowIfNull(groupEntity.CartrigesAndVaporizers, nameof(groupEntity.CartrigesAndVaporizers));

            ArgumentNullException.ThrowIfNull(groupEntity.CartrigesAndVaporizers.First().Good, "Good");

            ShortGroupDTO groupDTO = BaseCreateShortGroup(groupEntity, CategoryEnum.CartrigesAndVaporizers);

            groupDTO.Producer = CreateProducer(groupEntity.Producer);

            Good good = groupEntity.CartrigesAndVaporizers.Select(x => x.Good).MinBy(x => x.Price);

            groupDTO.Price = good.Price;
            groupDTO.DiscountPrice = _discountsService.GetDiscountPrice(good.DiscountGood);

            return groupDTO;
        }

        public CartrigesAndVaporizer CreateGoodEntity(CreateCartrigeAndVaporizerGoodDTO dto)
        {
            return new CartrigesAndVaporizer
            {
                Good = base.CreateGoodEntity(dto, CategoryEnum.CartrigesAndVaporizers),
                GroupId = dto.GroupId,
                Resistance = dto.Resistance
            };
        }

        public CartrigesAndVaporizersGroup CreateGroupEntity(CreateCartrigeAndVaporizerGroupDTO dto)
        {
            return new CartrigesAndVaporizersGroup
            {
                CartrigeCapacity = dto.CartrigeCapacity,
                SpiralType = dto.SpiralType,
                IsVaporizer = dto.IsVaporizer,
                Line = dto.Line,
                Name = dto.Name,
                ImageId = dto.ImageId,
                ProducerId = dto.ProducerId
            };
        }

        public IQueryable<CartrigesAndVaporizersGroup> IncludeForShortGroup(IQueryable<CartrigesAndVaporizersGroup> groups)
        {
            //!!! If changed, change IncludeForGroup in PodDtoService !!!

            return groups
                .Include(x => x.Producer)
                .Include(x => x.CartrigesAndVaporizers)
                    .ThenInclude(g => g.Good)
                        .ThenInclude(x => x.DiscountGood);

            //!!! If changed, change IncludeForGroup in PodDtoService !!!
        }

        public IQueryable<CartrigesAndVaporizersGroup> IncludeForGroup(IQueryable<CartrigesAndVaporizersGroup> groups)
        {
            return groups
                .Include(x => x.Producer)
                .Include(x => x.CartrigesAndVaporizers)
                    .ThenInclude(g => g.Good)
                        .ThenInclude(x => x.DiscountGood)

                .Include(x => x.Pods)
                    .ThenInclude(x => x.Producer)
                .Include(x => x.Pods)
                    .ThenInclude(x => x.Pods)
                        .ThenInclude(g => g.Good)
                            .ThenInclude(x => x.DiscountGood);
        }

        public IQueryable<CartrigesAndVaporizer> IncludeForGood(IQueryable<CartrigesAndVaporizer> goods)
        {
            return goods
                .Include(x => x.Group)
                    .ThenInclude(x => x.Producer)
                .Include(x => x.Good)
                    .ThenInclude(x => x.DiscountGood);
        }
    }
}
