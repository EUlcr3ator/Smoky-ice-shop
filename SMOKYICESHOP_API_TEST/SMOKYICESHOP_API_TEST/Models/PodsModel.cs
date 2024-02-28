using SMOKYICESHOP_API_TEST.DbContexts;
using SMOKYICESHOP_API_TEST.Entities;
using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DtoService;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DTO.Filters;
using System.Linq;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class PodsModel
    {
        private readonly PodDtoService _podDtoService;
        private readonly DefaultDtoService _defaultDtoService;
        private readonly SmokyIceDbContext _dbcontext;

        public PodsModel(PodDtoService podDtoService, SmokyIceDbContext dbcontext, DefaultDtoService defaultDtoService)
        {
            _podDtoService = podDtoService;
            _dbcontext = dbcontext;
            _defaultDtoService = defaultDtoService;
        }

        public GroupsCollection GetAllPods()
        {
            var collection = new GroupsCollection();

            collection.Groups = _podDtoService
                .IncludeForShortGroup(_dbcontext.PodsGroups)
                .OrderBy(x => x.Producer.Name)
                .Select(x => _podDtoService.CreateShortGroupDto(x))
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

        public GroupsCollection GetAllPods(int pageSize, int pageNumber)
        {
            var collection = new GroupsCollection();

            IQueryable<PodsGroup> groups = _podDtoService
                .IncludeForShortGroup(_dbcontext.PodsGroups)
                .OrderBy(x => x.Producer.Name);

            collection.Groups = groups
                .Select(x => _podDtoService.CreateShortGroupDto(x))
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

        public GoodsCollection GetAllPods(int pageSize, int pageNumber, PodFilterDTO filter)
        {
            var collection = new GoodsCollection();

            IQueryable<Pod> goods = _podDtoService
                .IncludeForGood(_dbcontext.Pods);

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

        private IQueryable<Pod> FilterSort(IQueryable<Pod> coals, PodFilterDTO filter)
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

        private IQueryable<Pod> ApplyFilter(IQueryable<Pod> goods, PodFilterDTO filter)
        {
            if (filter.MaxPrice != null)
                goods = goods.Where(x => filter.MaxPrice >= x.Good.Price);

            if (filter.MinPrice != null)
                goods = goods.Where(x => filter.MinPrice <= x.Good.Price);

            if (filter.ProducerNames != null)
                goods = goods.Where(x => filter.ProducerNames.Contains(x.Group.Producer.Name));

            if (filter.Materials != null)
                goods = goods.Where(x => filter.Materials.Contains(x.Group.Material));

            if (filter.EvaporatorResistances != null)
                goods = goods.Where(x => filter.EvaporatorResistances.Any(y => y == x.Group.EvaporatorResistance));

            if (filter.Battareys != null)
                goods = goods.Where(x => filter.Battareys.Contains(x.Group.Battarey));

            if (filter.CartrigeCapacities != null)
                goods = goods.Where(x => filter.CartrigeCapacities.Any(y => y == x.Group.CartrigeCapacity));

            return goods;
        }

        public PodGroupDTO GetGroup(Guid groupId)
        {
            PodsGroup group = _podDtoService
                .IncludeForGroup(_dbcontext.PodsGroups)
                .First(x => x.Id == groupId);

            return _podDtoService.CreateGroupDto(group);
        }

        public Guid AddGroup(CreatePodGroupDTO group)
        {
            PodsGroup entity = _podDtoService.CreateGroupEntity(group);
            _dbcontext.Add(entity);
            _dbcontext.SaveChanges();

            return entity.Id;
        }

        public bool RemoveGroup(Guid groupId)
        {
            PodsGroup group;

            try
            {
                group = _dbcontext.PodsGroups
                .Include(x => x.Pods)
                .First(x => x.Id == groupId);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            if (group.Pods.Count() != 0)
                throw new InvalidOperationException();

            _dbcontext.PodsGroups.Remove(group);
            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateGroup(CreatePodGroupDTO group, Guid groupId)
        {
            PodsGroup entity = _podDtoService.CreateGroupEntity(group);
            entity.Id = groupId;

            _dbcontext.PodsGroups.Update(entity);
            _dbcontext.SaveChanges();
            return true;
        }

        public Guid AddGood(CreatePodGoodDTO good)
        {
            Pod pod = _podDtoService.CreateGoodEntity(good);
            _dbcontext.Pods.Add(pod);
            _dbcontext.SaveChanges();

            return pod.GoodId;
        }

        public bool RemoveGood(Guid goodId)
        {
            Pod pod;
            try
            {
                pod = _dbcontext.Pods
                    .Include(x => x.Good)
                        .ThenInclude(x => x.DiscountGood)
                    .First(x => x.GoodId == goodId);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            Good good = pod.Good;
            DiscountGood? discountGood = pod.Good.DiscountGood;

            if (discountGood != null)
                _dbcontext.DiscountGoods.Remove(discountGood);
            _dbcontext.Pods.Remove(pod);
            _dbcontext.Goods.Remove(good);

            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateGood(CreatePodGoodDTO good, Guid goodId)
        {
            try
            {
                Pod pod = _podDtoService.CreateGoodEntity(good);
                pod.Good.Id = goodId;
                pod.GoodId = goodId;

                _dbcontext.Pods.Update(pod);
                _dbcontext.Goods.Update(pod.Good);

                _dbcontext.SaveChanges();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public IEnumerable<double> GetWeights()
        {
            return _dbcontext.PodsGroups
                .Select(x => x.Weight)
                .Distinct()
                .ToList();
        }

        public IEnumerable<string> GetMaterials()
        {
            return _dbcontext.PodsGroups
                .Select(x => x.Material)
                .Distinct()
                .ToList();
        }

        public IEnumerable<double> GetEvaporatorResistances()
        {
            return _dbcontext.PodsGroups
                .Select(x => x.EvaporatorResistance)
                .Distinct()
                .ToList();
        }

        public IEnumerable<string> GetPowers()
        {
            return _dbcontext.PodsGroups
                .Select(x => x.Power)
                .Distinct()
                .ToList();
        }

        public IEnumerable<short> GetBattareys()
        {
            return _dbcontext.PodsGroups
                .Select(x => x.Battarey)
                .Distinct()
                .ToList();
        }

        public IEnumerable<double> GetCartrigeCapacities()
        {
            return _dbcontext.PodsGroups
                .Select(x => x.CartrigeCapacity)
                .Distinct()
                .ToList();
        }

        public IEnumerable<string> GetPorts()
        {
            return _dbcontext.PodsGroups
                .Select(x => x.Port)
                .Distinct()
                .ToList();
        }

        public IEnumerable<string> GetAppearances()
        {
            return _dbcontext.Pods
                .Select(x => x.Appearance)
                .Distinct()
                .ToList();
        }
    }
}
