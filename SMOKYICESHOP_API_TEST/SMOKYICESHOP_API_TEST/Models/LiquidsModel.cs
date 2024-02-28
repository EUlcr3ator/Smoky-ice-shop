using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;
using SMOKYICESHOP_API_TEST.DTO.FieldValues;
using SMOKYICESHOP_API_TEST.DTO.Filters;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DtoService;
using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class LiquidsModel
    {
        private readonly LiquidDtoService _liquidDtoService;
        private readonly DefaultDtoService _defaultDtoService;
        private readonly SmokyIceDbContext _dbcontext;

        public LiquidsModel(SmokyIceDbContext dbcontext, LiquidDtoService liquidDtoService, DefaultDtoService defaultDtoService)
        {
            _dbcontext = dbcontext;
            _liquidDtoService = liquidDtoService;
            _defaultDtoService = defaultDtoService;
        }

        public GroupsCollection GetAllLiquids()
        {
            var collection = new GroupsCollection();

            collection.Groups = _liquidDtoService
                .IncludeForShortGroup(_dbcontext.LiquidsGroups)
                .OrderBy(x => x.Producer.Name)
                .Select(x => _liquidDtoService.CreateShortGroupDto(x))
                .ToList();

            if (collection.Groups.Count() != 0)
            {
                collection.MaxPrice = collection.Groups.Max(x => x.Price);
                collection.MinPrice = collection.Groups.Min(x => x.Price);
            }

            collection.TotalPages = 1;
            collection.IsLastPage = true;

            return collection;
        }

        public GroupsCollection GetAllLiquids(int pageSize, int pageNumber)
        {
            var collection = new GroupsCollection();

            IQueryable<LiquidsGroup> groups = _liquidDtoService
                .IncludeForShortGroup(_dbcontext.LiquidsGroups)
                .OrderBy(x => x.Producer.Name);

            collection.Groups = groups
                .Select(x => _liquidDtoService.CreateShortGroupDto(x))
                .ToList();

            int groupsCount = groups.Count();
            decimal pages = Decimal.Divide(groupsCount, pageSize);
            pages = Decimal.Round(pages, MidpointRounding.ToPositiveInfinity);

            if (groupsCount != 0)
            {
                collection.MaxPrice = collection.Groups.Max(x => x.Price);
                collection.MinPrice = collection.Groups.Min(x => x.Price);
            }

            collection.TotalPages = Decimal.ToInt32(pages);
            collection.IsLastPage = collection.TotalPages <= pageNumber;

            collection.Groups = collection.Groups
                .Skip(pageSize * pageNumber - pageSize)
                .Take(pageSize);

            return collection;
        }

        public GoodsCollection GetAllLiquids(int pageSize, int pageNumber, LiquidFilterDTO filter)
        {
            var collection = new GoodsCollection();

            IQueryable<Liquid> entities = _liquidDtoService
                .IncludeForGood(_dbcontext.Liquids);

            int goodsCount = entities.Count();
            decimal pages = Decimal.Divide(goodsCount, pageSize);
            pages = Decimal.Round(pages, MidpointRounding.ToPositiveInfinity);

            if (goodsCount != 0)
            {
                collection.MaxPrice = entities.Max(x => x.Good.Price);
                collection.MinPrice = entities.Min(x => x.Good.Price);
            }

            collection.TotalPages = Decimal.ToInt32(pages);
            collection.IsLastPage = collection.TotalPages <= pageNumber;

            entities = FilterSort(entities, filter);
            entities = ApplyFilter(entities, filter)
                .Skip(pageSize * pageNumber - pageSize)
                .Take(pageSize);

            collection.Goods = entities
                .Select(x => _defaultDtoService.CreateDefaultGood(x.Good))
                .ToList();

            return collection;
        }

        private IQueryable<Liquid> FilterSort(IQueryable<Liquid> liquids, LiquidFilterDTO filter)
        {
            switch (filter.SortToCheap)
            {
                case true:
                    return liquids.OrderByDescending(x => x.Good.Price);
                case false:
                    return liquids.OrderBy(x => x.Good.Price);
                default:
                    return liquids.OrderByDescending(x => x.Group.Producer.Name);
            }
        }

        private IQueryable<Liquid> ApplyFilter(IQueryable<Liquid> goods, LiquidFilterDTO filter)
        {
            if (filter.MaxPrice != null)
                goods = goods.Where(x => filter.MaxPrice >= x.Good.Price);

            if (filter.MinPrice != null)
                goods = goods.Where(x => filter.MinPrice <= x.Good.Price);

            if (filter.ProducerNames != null)
                goods = goods.Where(x => filter.ProducerNames.Contains(x.Group.Producer.Name));

            if (filter.NicotineStrengths != null)
                goods = goods.Where(x => filter.NicotineStrengths.Contains(x.Strength));

            if (filter.Capacities != null)
                goods = goods.Where(x => filter.Capacities.Contains(x.Group.Capacity));

            if (filter.TasteGroups != null)
                goods = goods.Where(x => x.TasteMix.Tastes.Any(y => filter.TasteGroups.Contains(y.TasteGroup)));

            if (filter.Tastes != null)
                goods = goods.Where(x => x.TasteMix.Tastes.Any(y => filter.Tastes.Contains(y.Taste)));

            return goods;
        }

        public LiquidGroupDTO GetGroup(Guid groupId)
        {
            LiquidsGroup group = _liquidDtoService
                .IncludeForGroup(_dbcontext.LiquidsGroups)
                .First(x => x.Id == groupId);

            return _liquidDtoService.CreateGroupDto(group);
        }

        public Guid AddGroup(CreateLiquidGroupDTO group)
        {
            LiquidsGroup entity = _liquidDtoService.CreateGroupEntity(group);
            _dbcontext.Add(entity);
            _dbcontext.SaveChanges();

            return entity.Id;
        }

        public bool RemoveGroup(Guid groupId)
        {
            LiquidsGroup group;

            try
            {
                group = _dbcontext.LiquidsGroups
                .Include(x => x.Liquids)
                .First(x => x.Id == groupId);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            if (group.Liquids.Count() != 0)
                throw new InvalidOperationException();

            _dbcontext.LiquidsGroups.Remove(group);
            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateGroup(CreateLiquidGroupDTO group, Guid groupId)
        {
            LiquidsGroup entity = _liquidDtoService.CreateGroupEntity(group);
            entity.Id = groupId;

            _dbcontext.LiquidsGroups.Update(entity);
            _dbcontext.SaveChanges();
            return true;
        }

        public Guid AddGood(CreateLiquidGoodDTO good)
        {
            Guid tasteMixId = _dbcontext.LiquidTasteMixes
                .First(x => x.Name == good.TasteMixName)
                .Id;

            Liquid entity = _liquidDtoService.CreateGoodEntity(good, tasteMixId);
            _dbcontext.Liquids.Add(entity);
            _dbcontext.SaveChanges();

            return entity.GoodId;
        }

        public bool RemoveGood(Guid goodId)
        {
            Liquid entity;
            try
            {
                entity = _dbcontext.Liquids
                    .Include(x => x.Good)
                        .ThenInclude(x => x.DiscountGood)
                    .First(x => x.GoodId == goodId);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            Good good = entity.Good;
            DiscountGood? discountGood = entity.Good.DiscountGood;

            if (discountGood != null)
                _dbcontext.DiscountGoods.Remove(discountGood);
            _dbcontext.Liquids.Remove(entity);
            _dbcontext.Goods.Remove(good);

            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateGood(CreateLiquidGoodDTO good, Guid goodId)
        {
            try
            {
                Guid tasteMixId = _dbcontext.LiquidTasteMixes
                    .First(x => x.Name == good.TasteMixName)
                    .Id;

                Liquid entity = _liquidDtoService.CreateGoodEntity(good, tasteMixId);
                entity.Good.Id = goodId;
                entity.GoodId = goodId;

                _dbcontext.Liquids.Update(entity);
                _dbcontext.Goods.Update(entity.Good);

                _dbcontext.SaveChanges();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public IEnumerable<byte> GetNicotineStrengths()
        {
            return _dbcontext.Liquids
                .Select(x => x.Strength)
                .Distinct()
                .ToList();
        }

        public IEnumerable<string> GetNicotineTypes()
        {
            return _dbcontext.LiquidsGroups
                .Select(x => x.NicotineType)
                .Distinct()
                .ToList();
        }

        public IEnumerable<byte> GetCapacities()
        {
            return _dbcontext.LiquidsGroups
                .Select(x => x.Capacity)
                .Distinct()
                .ToList();
        }

        public IEnumerable<string> GetTasteGroups()
        {
            return _dbcontext.LiquidsTastes
                .Select(x => x.TasteGroup)
                .Distinct()
                .ToList();
        }

        public IEnumerable<string> GetTastes()
        {
            return _dbcontext.LiquidsTastes
                .Select(x => x.Taste)
                .Distinct()
                .ToList();
        }

        public Guid AddTasteMix(TasteMixDTO<TasteDTO> tasteMix)
        {
            LiquidTasteMix? liquidTasteMix = _dbcontext.LiquidTasteMixes.FirstOrDefault(x => x.Name == tasteMix.Name);

            if (liquidTasteMix == null)
            {
                liquidTasteMix = new LiquidTasteMix
                {
                    Name = tasteMix.Name
                };

                foreach(TasteDTO taste in tasteMix.Tastes)
                {
                    LiquidsTaste? liquidsTaste = _dbcontext.LiquidsTastes
                        .FirstOrDefault(x => x.Taste == taste.Taste);
                    
                    if (liquidsTaste == null)
                    {
                        liquidsTaste = new LiquidsTaste
                        {
                            Taste = taste.Taste,
                            TasteGroup = taste.TasteGroup
                        };
                    }

                    liquidTasteMix.Tastes.Add(liquidsTaste);
                }

                _dbcontext.LiquidTasteMixes.Add(liquidTasteMix);
                _dbcontext.SaveChanges();
            }

            return liquidTasteMix.Id;
        }
    }
}
