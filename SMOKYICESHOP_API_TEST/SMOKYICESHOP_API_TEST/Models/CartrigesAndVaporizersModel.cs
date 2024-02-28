using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DbContexts;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;
using SMOKYICESHOP_API_TEST.DTO.Filters;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DtoService;
using SMOKYICESHOP_API_TEST.Entities;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class CartrigesAndVaporizersModel
    {
        private readonly CartrigeDtoService _cartrigeDtoService;
        private readonly DefaultDtoService _defaultDtoService;
        private readonly SmokyIceDbContext _dbcontext;

        public CartrigesAndVaporizersModel(CartrigeDtoService cartrigeDtoService, SmokyIceDbContext dbcontext, DefaultDtoService defaultDtoService)
        {
            _cartrigeDtoService = cartrigeDtoService;
            _dbcontext = dbcontext;
            _defaultDtoService = defaultDtoService;
        }

        public GroupsCollection GetAllCartriges()
        {
            var collection = new GroupsCollection();

            collection.Groups = _cartrigeDtoService
                .IncludeForShortGroup(_dbcontext.CartrigesAndVaporizersGroups)
                .OrderBy(x => x.Producer.Name)
                .Select(x => _cartrigeDtoService.CreateShortGroupDto(x))
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

        public GroupsCollection GetAllCartriges(int pageSize, int pageNumber)
        {
            var collection = new GroupsCollection();

            IQueryable<CartrigesAndVaporizersGroup> groups = _cartrigeDtoService
                .IncludeForShortGroup(_dbcontext.CartrigesAndVaporizersGroups)
                .OrderBy(x => x.Producer.Name);

            collection.Groups = groups
                .Select(x => _cartrigeDtoService.CreateShortGroupDto(x))
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

        public GoodsCollection GetAllCartriges(int pageSize, int pageNumber, CartrigeFilterDTO filter)
        {
            var collection = new GoodsCollection();

            IQueryable<CartrigesAndVaporizer> cartriges = _cartrigeDtoService
                .IncludeForGood(_dbcontext.CartrigesAndVaporizers);

            int goodsCount = cartriges.Count();
            decimal pages = Decimal.Divide(goodsCount, pageSize);
            pages = Decimal.Round(pages, MidpointRounding.ToPositiveInfinity);

            if (goodsCount != 0)
            {
                collection.MaxPrice = cartriges.Max(x => x.Good.Price);
                collection.MinPrice = cartriges.Min(x => x.Good.Price);
            }

            collection.TotalPages = Decimal.ToInt32(pages);
            collection.IsLastPage = collection.TotalPages <= pageNumber;

            cartriges = FilterSort(cartriges, filter);
            cartriges = ApplyFilter(cartriges, filter)
                .Skip(pageSize * pageNumber - pageSize)
                .Take(pageSize);

            collection.Goods = cartriges
                .Select(x => _defaultDtoService.CreateDefaultGood(x.Good))
                .ToList();

            return collection;
        }

        private IQueryable<CartrigesAndVaporizer> FilterSort(IQueryable<CartrigesAndVaporizer> coals, CartrigeFilterDTO filter)
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

        private IQueryable<CartrigesAndVaporizer> ApplyFilter(IQueryable<CartrigesAndVaporizer> goods, CartrigeFilterDTO filter)
        {
            if (filter.MaxPrice != null)
                goods = goods.Where(x => filter.MaxPrice >= x.Good.Price);

            if (filter.MinPrice != null)
                goods = goods.Where(x => filter.MinPrice <= x.Good.Price);

            if (filter.ProducerNames != null)
                goods = goods.Where(x => filter.ProducerNames.Contains(x.Group.Producer.Name));

            if (filter.SpiralTypes != null)
                goods = goods.Where(x => filter.SpiralTypes.Contains(x.Group.SpiralType));

            if (filter.CartrigeCapacities != null)
                goods = goods.Where(x => filter.CartrigeCapacities.Any(y => y == x.Group.CartrigeCapacity));

            if (filter.Resistances != null)
                goods = goods.Where(x => filter.Resistances.Contains(x.Resistance));

            return goods;
        }

        public CartrigeAndVaporizerGroupDTO GetGroup(Guid groupId)
        {
            CartrigesAndVaporizersGroup group = _cartrigeDtoService
                .IncludeForGroup(_dbcontext.CartrigesAndVaporizersGroups)
                .First(x => x.Id == groupId);

            return _cartrigeDtoService.CreateGroupDto(group);
        }

        public Guid AddGroup(CreateCartrigeAndVaporizerGroupDTO group)
        {
            CartrigesAndVaporizersGroup entity = _cartrigeDtoService.CreateGroupEntity(group);
            _dbcontext.Add(entity);
            _dbcontext.SaveChanges();

            return entity.Id;
        }

        public bool RemoveGroup(Guid groupId)
        {
            CartrigesAndVaporizersGroup group;

            try
            {
                group = _dbcontext.CartrigesAndVaporizersGroups
                .Include(x => x.CartrigesAndVaporizers)
                .First(x => x.Id == groupId);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            if (group.CartrigesAndVaporizers.Count() != 0)
                throw new InvalidOperationException();

            _dbcontext.CartrigesAndVaporizersGroups.Remove(group);
            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateGroup(CreateCartrigeAndVaporizerGroupDTO group, Guid groupId)
        {
            CartrigesAndVaporizersGroup entity = _cartrigeDtoService.CreateGroupEntity(group);
            entity.Id = groupId;

            _dbcontext.CartrigesAndVaporizersGroups.Update(entity);
            _dbcontext.SaveChanges();
            return true;
        }

        public Guid AddGood(CreateCartrigeAndVaporizerGoodDTO good)
        {
            CartrigesAndVaporizer cartrige = _cartrigeDtoService.CreateGoodEntity(good);
            _dbcontext.CartrigesAndVaporizers.Add(cartrige);
            _dbcontext.SaveChanges();

            return cartrige.GoodId;
        }

        public bool RemoveGood(Guid goodId)
        {
            CartrigesAndVaporizer cartrige;
            try
            {
                cartrige = _dbcontext.CartrigesAndVaporizers
                    .Include(x => x.Good)
                        .ThenInclude(x => x.DiscountGood)
                    .First(x => x.GoodId == goodId);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            Good good = cartrige.Good;
            DiscountGood? discountGood = cartrige.Good.DiscountGood;

            if (discountGood != null)
                _dbcontext.DiscountGoods.Remove(discountGood);
            _dbcontext.CartrigesAndVaporizers.Remove(cartrige);
            _dbcontext.Goods.Remove(good);
            
            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateGood(CreateCartrigeAndVaporizerGoodDTO good, Guid goodId)
        {
            try
            {
                CartrigesAndVaporizer cartrige = _cartrigeDtoService.CreateGoodEntity(good);
                cartrige.Good.Id = goodId;
                cartrige.GoodId = goodId;

                _dbcontext.CartrigesAndVaporizers.Update(cartrige);
                _dbcontext.Goods.Update(cartrige.Good);

                _dbcontext.SaveChanges();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public IEnumerable<string> GetSpiralTypes()
        {
            return _dbcontext.CartrigesAndVaporizersGroups
                .Select(x => x.SpiralType)
                .Distinct()
                .Where(x => x != null)
                .ToList();
        }

        public IEnumerable<double> GetCartrigeCapacities()
        {
            return _dbcontext.CartrigesAndVaporizersGroups
                .Select(x => x.CartrigeCapacity)
                .ToList()
                .Distinct()
                .OfType<double>()
                .ToList();
        }

        public IEnumerable<double> GetResistances()
        {
            return _dbcontext.CartrigesAndVaporizers
                .Select(x => x.Resistance)
                .Distinct()
                .ToList();
        }
    }
}
