using SMOKYICESHOP_API_TEST.DbContexts;
using SMOKYICESHOP_API_TEST.Entities;
using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DtoService;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DTO.Filters;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class CoalsModel
    {
        private readonly CoalDtoService _coalDtoService;
        private readonly DefaultDtoService _defaultDtoService;
        private readonly SmokyIceDbContext _dbcontext;

        public CoalsModel(CoalDtoService coalDtoService, SmokyIceDbContext dbcontext, DefaultDtoService defaultDtoService)
        {
            _coalDtoService = coalDtoService;
            _dbcontext = dbcontext;
            _defaultDtoService = defaultDtoService;
        }

        public GroupsCollection GetAllCoals()
        {
            var collection = new GroupsCollection();

            collection.Groups = _coalDtoService
                .IncludeForShortGroup(_dbcontext.CoalsGroups)
                .OrderBy(x => x.Producer.Name)
                .Select(x => _coalDtoService.CreateShortGroupDto(x))
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

        public GroupsCollection GetAllCoals(int pageSize, int pageNumber)
        {
            var collection = new GroupsCollection();

            IQueryable<CoalsGroup> groups = _coalDtoService
                .IncludeForShortGroup(_dbcontext.CoalsGroups)
                .OrderBy(x => x.Producer.Name);

            collection.Groups = groups
                .Select(x => _coalDtoService.CreateShortGroupDto(x))
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

        public GoodsCollection GetAllCoals(int pageSize, int pageNumber, CoalFilterDTO filter)
        {
            var collection = new GoodsCollection();

            IQueryable<Coal> goods = _coalDtoService
                .IncludeForGood(_dbcontext.Coals);

            int goodsCount = goods.Count();
            decimal pages = Decimal.Divide(goodsCount, pageSize);
            pages = Decimal.Round(pages, MidpointRounding.ToPositiveInfinity);

            if (goodsCount != 0)
            {
                collection.MaxPrice = goods.Max(x => x.Good.Price);
                collection.MinPrice = goods.Min(x => x.Good.Price);
            }

            collection.TotalPages = Decimal.ToInt32(pages);
            collection.IsLastPage = collection.TotalPages <= pageNumber;

            goods = FilterSort(goods, filter);
            goods = ApplyFilter(goods, filter)
                .Skip(pageSize * pageNumber - pageSize)
                .Take(pageSize);

            collection.Goods = goods
                .Select(x => _defaultDtoService.CreateDefaultGood(x.Good))
                .ToList();

            return collection;
        }

        private IQueryable<Coal> FilterSort(IQueryable<Coal> coals, CoalFilterDTO filter)
        {
            switch (filter.SortToCheap)
            {
                case true:
                    return coals.OrderByDescending(x => x.Good.Price);
                case false:
                    return coals.OrderBy(x => x.Good.Price);
                default:
                    return coals.OrderByDescending(x => x.Group.Producer.Name);
            }
        }

        private IQueryable<Coal> ApplyFilter(IQueryable<Coal> goods, CoalFilterDTO filter)
        {
            if (filter.MaxPrice != null)
                goods = goods.Where(x => filter.MaxPrice >= x.Good.Price);

            if (filter.MinPrice != null)
                goods = goods.Where(x => filter.MinPrice <= x.Good.Price);

            if (filter.ProducerNames != null)
                goods = goods.Where(x => filter.ProducerNames.Contains(x.Group.Producer.Name));

            if (filter.Types != null)
                goods = goods.Where(x => filter.Types.Contains(x.Group.Type));

            if (filter.Weights != null)
                goods = goods.Where(x => filter.Weights.Contains(x.Weight));

            return goods;
        }

        public CoalGroupDTO GetGroup(Guid groupId)
        {
            CoalsGroup group = _coalDtoService
                .IncludeForGroup(_dbcontext.CoalsGroups)
                .First(x => x.Id == groupId);

            return _coalDtoService.CreateGroupDto(group);
        }

        public Guid AddGroup(CreateCoalGroupDTO group)
        {
            CoalsGroup entity = _coalDtoService.CreateGroupEntity(group);
            _dbcontext.Add(entity);
            _dbcontext.SaveChanges();

            return entity.Id;
        }

        public bool RemoveGroup(Guid groupId)
        {
            CoalsGroup group;

            try
            {
                group = _dbcontext.CoalsGroups
                .Include(x => x.Coals)
                .First(x => x.Id == groupId);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            if (group.Coals.Count() != 0)
                throw new InvalidOperationException();

            _dbcontext.CoalsGroups.Remove(group);
            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateGroup(CreateCoalGroupDTO group, Guid groupId)
        {
            CoalsGroup entity = _coalDtoService.CreateGroupEntity(group);
            entity.Id = groupId;

            _dbcontext.CoalsGroups.Update(entity);
            _dbcontext.SaveChanges();
            return true;
        }

        public Guid AddGood(CreateCoalGoodDTO good)
        {
            Coal coal = _coalDtoService.CreateGoodEntity(good);
            _dbcontext.Coals.Add(coal);
            _dbcontext.SaveChanges();

            return coal.GoodId;
        }

        public bool RemoveGood(Guid goodId)
        {
            Coal coal;
            try
            {
                coal = _dbcontext.Coals
                    .Include(x => x.Good)
                        .ThenInclude(x => x.DiscountGood)
                    .First(x => x.GoodId == goodId);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            Good good = coal.Good;
            DiscountGood? discountGood = coal.Good.DiscountGood;

            if (discountGood != null)
                _dbcontext.DiscountGoods.Remove(discountGood);
            _dbcontext.Coals.Remove(coal);
            _dbcontext.Goods.Remove(good);

            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateGood(CreateCoalGoodDTO good, Guid goodId)
        {
            try
            {
                Coal coal = _coalDtoService.CreateGoodEntity(good);
                coal.Good.Id = goodId;
                coal.GoodId = goodId;

                _dbcontext.Coals.Update(coal);
                _dbcontext.Goods.Update(coal.Good);

                _dbcontext.SaveChanges();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public IEnumerable<string> GetTypes()
        {
            return _dbcontext.CoalsGroups
                .Select(x => x.Type)
                .Distinct()
                .ToList();
        }

        public IEnumerable<double> GetWeights()
        {
            return _dbcontext.Coals
                .Select(x => x.Weight)
                .Distinct()
                .ToList();
        }
    }
}
