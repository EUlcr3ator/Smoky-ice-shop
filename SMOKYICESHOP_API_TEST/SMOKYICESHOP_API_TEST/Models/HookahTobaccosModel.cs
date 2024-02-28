using SMOKYICESHOP_API_TEST.Entities;
using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DtoService;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DTO.Filters;
using System.Linq;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;
using SMOKYICESHOP_API_TEST.DTO.FieldValues;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class HookahTobaccosModel
    {
        private readonly HookahTobaccoDtoService _tobaccoDtoService;
        private readonly DefaultDtoService _defaultDtoService;
        private readonly SmokyIceDbContext _dbcontext;

        public HookahTobaccosModel(HookahTobaccoDtoService tobaccoDtoService, SmokyIceDbContext dbcontext, DefaultDtoService defaultDtoService)
        {
            _tobaccoDtoService = tobaccoDtoService;
            _dbcontext = dbcontext;
            _defaultDtoService = defaultDtoService;
        }

        public GroupsCollection GetAllTobaccos()
        {
            var collection = new GroupsCollection();

            collection.Groups = _tobaccoDtoService
                .IncludeForShortGroup(_dbcontext.HookahTobaccoGroups)
                .OrderBy(x => x.Producer.Name)
                .Select(x => _tobaccoDtoService.CreateShortGroupDto(x))
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

        public GroupsCollection GetAllTobaccos(int pageSize, int pageNumber)
        {
            var collection = new GroupsCollection();

            IQueryable<HookahTobaccoGroup> groups = _tobaccoDtoService
                .IncludeForShortGroup(_dbcontext.HookahTobaccoGroups)
                .OrderBy(x => x.Producer.Name);

            collection.Groups = groups
                .Select(x => _tobaccoDtoService.CreateShortGroupDto(x))
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

        public GoodsCollection GetAllTobaccos(int pageSize, int pageNumber, TobaccoFilterDTO filter)
        {
            var collection = new GoodsCollection();

            IQueryable<HookahTobacco> cartriges = _tobaccoDtoService
                .IncludeForGood(_dbcontext.HookahTobaccos);

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

        private IQueryable<HookahTobacco> FilterSort(IQueryable<HookahTobacco> tobaccos, TobaccoFilterDTO filter)
        {
            switch (filter.SortToCheap)
            {
                case true:
                    return tobaccos.OrderByDescending(x => x.Good.Price);
                case false:
                    return tobaccos.OrderBy(x => x.Good.Price);
                default:
                    return tobaccos.OrderByDescending(x => x.Group.Producer.Name);
            }
        }

        private IQueryable<HookahTobacco> ApplyFilter(IQueryable<HookahTobacco> goods, TobaccoFilterDTO filter)
        {
            if (filter.MaxPrice != null)
                goods = goods.Where(x => filter.MaxPrice >= x.Good.Price);

            if (filter.MinPrice != null)
                goods = goods.Where(x => filter.MinPrice <= x.Good.Price);

            if (filter.ProducerNames != null)
                goods = goods.Where(x => filter.ProducerNames.Contains(x.Group.Producer.Name));

            if (filter.Sweet != null)
                goods = goods.Where(x => x.Group.Sweet <= filter.Sweet);

            if (filter.Sour != null)
                goods = goods.Where(x => x.Group.Sour <= filter.Sour);

            if (filter.Fresh != null)
                goods = goods.Where(x => x.Group.Fresh <= filter.Fresh);

            if (filter.Spicy != null)
                goods = goods.Where(x => x.Group.Spicy <= filter.Spicy);

            if (filter.Tastes != null)
                goods = goods.Where(x => x.Group.TasteMix.Tastes.Any(y => filter.Tastes.Contains(y.Taste)));

            if (filter.Strengths != null)
                goods = goods.Where(x => filter.Strengths.Contains(x.Group.Strength.Name));

            if (filter.Weights != null)
                goods = goods.Where(x => filter.Weights.Any(y => y == x.Weight));

            return goods;
        }

        public HookahTobaccoGroupDTO GetGroup(Guid groupId)
        {
            HookahTobaccoGroup group = _tobaccoDtoService
                .IncludeForGroup(_dbcontext.HookahTobaccoGroups)
                .First(x => x.Id == groupId);

            return _tobaccoDtoService.CreateGroupDto(group);
        }

        public Guid AddGroup(CreateHookahTobaccoGroupDTO group)
        {
            Guid tasteMixId = _dbcontext.HookahTobaccoTasteMixes
                .First(x => x.Name == group.TasteMixName)
                .Id;

            byte strengthId = _dbcontext.HookahTobaccoStrengths
                .First(x => x.Name == group.Strength)
                .Id;

            HookahTobaccoGroup entity = _tobaccoDtoService.CreateGroupEntity(group, tasteMixId, strengthId);
            _dbcontext.Add(entity);
            _dbcontext.SaveChanges();

            return entity.Id;
        }

        public bool RemoveGroup(Guid groupId)
        {
            HookahTobaccoGroup group;

            try
            {
                group = _dbcontext.HookahTobaccoGroups
                .Include(x => x.HookahTobaccos)
                .First(x => x.Id == groupId);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            if (group.HookahTobaccos.Count() != 0)
                throw new InvalidOperationException();

            _dbcontext.HookahTobaccoGroups.Remove(group);
            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateGroup(CreateHookahTobaccoGroupDTO group, Guid groupId)
        {
            Guid tasteMixId = _dbcontext.HookahTobaccoTastes
                .First(x => x.Taste == group.TasteMixName)
                .Id;

            byte strengthId = _dbcontext.HookahTobaccoStrengths
                .First(x => x.Name == group.Strength)
                .Id;

            HookahTobaccoGroup entity = _tobaccoDtoService.CreateGroupEntity(group, tasteMixId, strengthId);
            entity.Id = groupId;

            _dbcontext.HookahTobaccoGroups.Update(entity);
            _dbcontext.SaveChanges();
            return true;
        }

        public Guid AddGood(CreateHookahTobaccoGoodDTO good)
        {
            HookahTobacco entity = _tobaccoDtoService.CreateGoodEntity(good);
            _dbcontext.HookahTobaccos.Add(entity);
            _dbcontext.SaveChanges();

            return entity.GoodId;
        }

        public bool RemoveGood(Guid goodId)
        {
            HookahTobacco entity;
            try
            {
                entity = _dbcontext.HookahTobaccos
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
            _dbcontext.HookahTobaccos.Remove(entity);
            _dbcontext.Goods.Remove(good);

            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateGood(CreateHookahTobaccoGoodDTO good, Guid goodId)
        {
            try
            {
                HookahTobacco entity = _tobaccoDtoService.CreateGoodEntity(good);
                entity.Good.Id = goodId;
                entity.GoodId = goodId;

                _dbcontext.HookahTobaccos.Update(entity);
                _dbcontext.Goods.Update(entity.Good);

                _dbcontext.SaveChanges();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public IEnumerable<string> GetStrengths()
        {
            return _dbcontext.HookahTobaccoStrengths
                .OrderBy(x => x.StrengthPoints)
                .Select(x => x.Name)
                .ToList();
        }

        public IEnumerable<double> GetWeights()
        {
            return _dbcontext.HookahTobaccos
                .Select(x => x.Weight)
                .Distinct()
                .ToList();
        }

        public IEnumerable<string> GetTastes()
        {
            return _dbcontext.HookahTobaccoTastes
                .Select(x => x.Taste)
                .ToList();
        }

        public Guid AddTasteMix(TasteMixDTO<string> tasteMix)
        {
            HookahTobaccoTasteMix? tobaccoTasteMix = _dbcontext.HookahTobaccoTasteMixes.FirstOrDefault(x => x.Name == tasteMix.Name);

            if (tobaccoTasteMix == null)
            {
                tobaccoTasteMix = new HookahTobaccoTasteMix
                {
                    Name = tasteMix.Name
                };

                foreach (string taste in tasteMix.Tastes)
                {
                    HookahTobaccoTaste? tobaccoTaste = _dbcontext.HookahTobaccoTastes
                        .FirstOrDefault(x => x.Taste == taste);

                    if (tobaccoTaste == null)
                    {
                        tobaccoTaste = new HookahTobaccoTaste
                        {
                            Taste = taste
                        };
                    }

                    tobaccoTasteMix.Tastes.Add(tobaccoTaste);
                }

                _dbcontext.HookahTobaccoTasteMixes.Add(tobaccoTasteMix);
                _dbcontext.SaveChanges();
            }

            return tobaccoTasteMix.Id;
        }
    }
}
