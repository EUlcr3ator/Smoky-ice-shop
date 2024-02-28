using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Services;

namespace SMOKYICESHOP_API_TEST.DtoService
{
    public class EcigaretteDtoService : BaseDtoService
    {
        private readonly DiscountsService _discountsService;

        public EcigaretteDtoService(DiscountsService discountsService)
        {
            _discountsService = discountsService;
        }

        public EcigaretteGroupDTO CreateGroupDto(EcigarettesGroup group)
        {
            ArgumentNullException.ThrowIfNull(group, nameof(group));
            ArgumentNullException.ThrowIfNull(group.Producer, nameof(group.Producer));

            EcigaretteGroupDTO item = new EcigaretteGroupDTO
            {
                GroupId = group.Id,
                EvaporatorVolume = group.EvaporatorVolume,
                BattareyCapacity = group.BatteryCapacity,
                PuffsCount = group.PuffsCount,
                Line = group.Line,
                Name = group.Name,
                Category = CategoryEnum.ECigarettes.ToString(),
                ImageId = group.ImageId,
                Producer = CreateProducer(group.Producer)
            };

            item.Goods = group.Ecigarettes.Select(x => CreateGoodDto(x));

            item.Tastes = group.Ecigarettes
                .GroupBy(x => x.TasteMix.Name)
                .Select(x => CreateIdGroup(x));

            return item;
        }

        public EcigaretteGoodDTO CreateGoodDto(Ecigarette entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            ArgumentNullException.ThrowIfNull(entity.Good, nameof(entity.Good));

            EcigaretteGoodDTO dto = new();

            short? discount = _discountsService.GetDiscountPrice(entity.Good.DiscountGood);
            base.FillGoodDto(dto, entity.Good, discount);

            dto.Sweet = entity.Sweet;
            dto.Sour = entity.Sour;
            dto.Fresh = entity.Fresh;
            dto.Spicy = entity.Spicy;

            dto.Taste = entity.TasteMix.Name;

            return dto;
        }

        public ShortGroupDTO CreateShortGroupDto(EcigarettesGroup groupEntity)
        {
            ArgumentNullException.ThrowIfNull(groupEntity, nameof(groupEntity));
            ArgumentNullException.ThrowIfNull(groupEntity.Producer, nameof(groupEntity.Producer));

            ArgumentNullException.ThrowIfNull(groupEntity.Ecigarettes.First().Good, "Good");

            ShortGroupDTO groupDTO = BaseCreateShortGroup(groupEntity, CategoryEnum.ECigarettes);

            groupDTO.Producer = CreateProducer(groupEntity.Producer);

            Good good = groupEntity.Ecigarettes.Select(x => x.Good).MinBy(x => x.Price);

            groupDTO.Price = good.Price;
            groupDTO.DiscountPrice = _discountsService.GetDiscountPrice(good.DiscountGood);

            return groupDTO;
        }

        public Ecigarette CreateGoodEntity(CreateEcigaretteGoodDTO dto, Guid tasteMixId)
        {
            var item = new Ecigarette
            {
                Good = base.CreateGoodEntity(dto, CategoryEnum.ECigarettes),
                GroupId = dto.GroupId,
                Sweet = dto.Sweet,
                Sour = dto.Sour,
                Fresh = dto.Fresh,
                Spicy = dto.Spicy
            };

            item.TasteMixId = tasteMixId;
            return item;
        }

        public EcigarettesGroup CreateGroupEntity(CreateEcigaretteGroupDTO dto)
        {
            return new EcigarettesGroup
            {
                EvaporatorVolume = dto.EvaporatorVolume,
                BatteryCapacity = dto.BattareyCapacity,
                PuffsCount = dto.PuffsCount,
                Line = dto.Line,
                Name = dto.Name,
                ImageId = dto.ImageId,
                ProducerId = dto.ProducerId
            };
        }

        public IQueryable<EcigarettesGroup> IncludeForShortGroup(IQueryable<EcigarettesGroup> groups)
        {
            return groups
                .Include(x => x.Producer)
                .Include(x => x.Ecigarettes)
                    .ThenInclude(g => g.TasteMix)
                .Include(x => x.Ecigarettes)
                    .ThenInclude(g => g.Good)
                        .ThenInclude(x => x.DiscountGood);
        }

        public IQueryable<EcigarettesGroup> IncludeForGroup(IQueryable<EcigarettesGroup> groups)
        {
            return groups
                .Include(x => x.Producer)
                .Include(x => x.Ecigarettes)
                    .ThenInclude(g => g.TasteMix)
                .Include(x => x.Ecigarettes)
                    .ThenInclude(g => g.Good)
                        .ThenInclude(x => x.DiscountGood);
        }

        public IQueryable<Ecigarette> IncludeForGood(IQueryable<Ecigarette> goods)
        {
            return goods
                .Include(x => x.Group)
                    .ThenInclude(x => x.Producer)
                .Include(x => x.TasteMix)
                .Include(x => x.Good)
                    .ThenInclude(x => x.DiscountGood);
        }
    }
}
