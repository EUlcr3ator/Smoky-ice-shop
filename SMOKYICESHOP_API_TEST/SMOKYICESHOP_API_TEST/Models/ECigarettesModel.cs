using Microsoft.EntityFrameworkCore;
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
    public class ECigarettesModel
    {
        private readonly EcigaretteDtoService _ecigaretteDtoService;
        private readonly DefaultDtoService _defaultDtoService;
        private readonly SmokyIceDbContext _dbcontext;

        public ECigarettesModel(EcigaretteDtoService ecigaretteDtoService, SmokyIceDbContext dbcontext, DefaultDtoService defaultDtoService)
        {
            _ecigaretteDtoService = ecigaretteDtoService;
            _dbcontext = dbcontext;
            _defaultDtoService = defaultDtoService;
        }

        public GroupsCollection GetAllCartriges()
        {
            var collection = new GroupsCollection();

            collection.Groups = _ecigaretteDtoService
                .IncludeForShortGroup(_dbcontext.EcigarettesGroups)
                .OrderBy(x => x.Producer.Name)
                .Select(x => _ecigaretteDtoService.CreateShortGroupDto(x))
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

            IQueryable<EcigarettesGroup> groups = _ecigaretteDtoService
                .IncludeForShortGroup(_dbcontext.EcigarettesGroups)
                .OrderBy(x => x.Producer.Name);

            collection.Groups = groups
                .Select(x => _ecigaretteDtoService.CreateShortGroupDto(x))
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

        public GoodsCollection GetAllCartriges(int pageSize, int pageNumber, ECigaretteFilterDTO filter)
        {
            var collection = new GoodsCollection();

            IQueryable<Ecigarette> eigarettes = _ecigaretteDtoService
                .IncludeForGood(_dbcontext.Ecigarettes);

            int goodsCount = eigarettes.Count();
            decimal pages = Decimal.Divide(goodsCount, pageSize);
            pages = Decimal.Round(pages, MidpointRounding.ToPositiveInfinity);

            if (goodsCount != 0)
            {
                collection.MaxPrice = eigarettes.Max(x => x.Good.Price);
                collection.MinPrice = eigarettes.Min(x => x.Good.Price);
            }

            collection.TotalPages = Decimal.ToInt32(pages);
            collection.IsLastPage = collection.TotalPages <= pageNumber;

            eigarettes = FilterSort(eigarettes, filter);
            eigarettes = ApplyFilter(eigarettes, filter)
                .Skip(pageSize * pageNumber - pageSize)
                .Take(pageSize);

            collection.Goods = eigarettes
                .Select(x => _defaultDtoService.CreateDefaultGood(x.Good))
                .ToList();

            return collection;
        }

        private IQueryable<Ecigarette> FilterSort(IQueryable<Ecigarette> goods, ECigaretteFilterDTO filter)
        {
            switch (filter.SortToCheap)
            {
                case true:
                    return goods.OrderByDescending(x => x.Good.Price);
                case false:
                    return goods.OrderBy(x => x.Good.Price);
                default:
                    return goods.OrderByDescending(x => x.Group.Producer.Name);
            }
        }

        private IQueryable<Ecigarette> ApplyFilter(IQueryable<Ecigarette> goods, ECigaretteFilterDTO filter)
        {
            if (filter.MaxPrice != null)
                goods = goods.Where(x => filter.MaxPrice >= x.Good.Price);

            if (filter.MinPrice != null)
                goods = goods.Where(x => filter.MinPrice <= x.Good.Price);

            if (filter.ProducerNames != null)
                goods = goods.Where(x => filter.ProducerNames.Contains(x.Group.Producer.Name));

            if (filter.Sweet != null)
                goods = goods.Where(x => x.Sweet <= filter.Sweet);

            if (filter.Sour != null)
                goods = goods.Where(x => x.Sour <= filter.Sour);

            if (filter.Fresh != null)
                goods = goods.Where(x => x.Fresh <= filter.Fresh);

            if (filter.Spicy != null)
                goods = goods.Where(x => x.Spicy <= filter.Spicy);

            if (filter.Tastes != null)
                goods = goods.Where(x => x.TasteMix.Tastes.Any(y => filter.Tastes.Contains(y.Name)));

            if (filter.PuffsCounts != null)
                goods = goods.Where(x => filter.PuffsCounts.Contains(x.Group.PuffsCount));

            return goods;
        }

        public EcigaretteGroupDTO GetGroup(Guid groupId)
        {
            EcigarettesGroup group = _ecigaretteDtoService
                .IncludeForGroup(_dbcontext.EcigarettesGroups)
                .First(x => x.Id == groupId);

            return _ecigaretteDtoService.CreateGroupDto(group);
        }

        public Guid AddGroup(CreateEcigaretteGroupDTO group)
        {
            EcigarettesGroup entity = _ecigaretteDtoService.CreateGroupEntity(group);
            _dbcontext.EcigarettesGroups.Add(entity);
            _dbcontext.SaveChanges();

            return entity.Id;
        }

        public bool RemoveGroup(Guid groupId)
        {
            EcigarettesGroup group;

            try
            {
                group = _dbcontext.EcigarettesGroups
                .Include(x => x.Ecigarettes)
                .First(x => x.Id == groupId);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            if (group.Ecigarettes.Count() != 0)
                throw new InvalidOperationException();

            _dbcontext.EcigarettesGroups.Remove(group);
            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateGroup(CreateEcigaretteGroupDTO group, Guid groupId)
        {
            EcigarettesGroup entity = _ecigaretteDtoService.CreateGroupEntity(group);
            entity.Id = groupId;

            _dbcontext.EcigarettesGroups.Update(entity);
            _dbcontext.SaveChanges();
            return true;
        }

        public Guid AddGood(CreateEcigaretteGoodDTO good)
        {
            Guid tasteMixId = _dbcontext.EcigaretteTasteMixes
                .First(x => x.Name == good.TasteMixName)
                .Id;

            Ecigarette cartrige = _ecigaretteDtoService.CreateGoodEntity(good, tasteMixId);
            _dbcontext.Ecigarettes.Add(cartrige);
            _dbcontext.SaveChanges();

            return cartrige.GoodId;
        }

        public bool RemoveGood(Guid goodId)
        {
            Ecigarette cigarette;
            try
            {
                cigarette = _dbcontext.Ecigarettes
                    .Include(x => x.Good)
                        .ThenInclude(x => x.DiscountGood)
                    .First(x => x.GoodId == goodId);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            Good good = cigarette.Good;
            DiscountGood? discountGood = cigarette.Good.DiscountGood;

            if (discountGood != null)
                _dbcontext.DiscountGoods.Remove(discountGood);
            _dbcontext.Ecigarettes.Remove(cigarette);
            _dbcontext.Goods.Remove(good);

            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateGood(CreateEcigaretteGoodDTO good, Guid goodId)
        {
            try
            {
                Guid tasteMixId = _dbcontext.EcigaretteTasteMixes
                    .First(x => x.Name == good.TasteMixName)
                    .Id;

                Ecigarette cigarette = _ecigaretteDtoService.CreateGoodEntity(good, tasteMixId);
                cigarette.Good.Id = goodId;
                cigarette.GoodId = goodId;

                _dbcontext.Ecigarettes.Update(cigarette);
                _dbcontext.Goods.Update(cigarette.Good);

                _dbcontext.SaveChanges();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public IEnumerable<short> GetBattareyCapacities()
        {
            return _dbcontext.EcigarettesGroups
                .Select(x => x.BatteryCapacity)
                .Distinct()
                .ToList();
        }

        public IEnumerable<string> GetTastes()
        {
            return _dbcontext.EcigarettesTastes
                .Select(x => x.Name)
                .Distinct()
                .ToList();
        }

        public IEnumerable<byte> GetEvaporatorVolumes()
        {
            return _dbcontext.EcigarettesGroups
                .Select(x => x.EvaporatorVolume)
                .Distinct()
                .ToList();
        }

        public IEnumerable<int> GetPuffsCounts()
        {
            return _dbcontext.EcigarettesGroups
                .Select(x => x.PuffsCount)
                .Distinct()
                .ToList();
        }

        public Guid AddTasteMix(TasteMixDTO<string> tasteMix)
        {
            EcigaretteTasteMix? ecigaretteTasteMix = _dbcontext.EcigaretteTasteMixes.FirstOrDefault(x => x.Name == tasteMix.Name);

            if (ecigaretteTasteMix == null)
            {
                ecigaretteTasteMix = new EcigaretteTasteMix
                {
                    Name = tasteMix.Name
                };

                foreach (string taste in tasteMix.Tastes)
                {
                    EcigarettesTaste? ecigaretteTaste = _dbcontext.EcigarettesTastes
                        .FirstOrDefault(x => x.Name == taste);

                    if (ecigaretteTaste == null)
                    {
                        ecigaretteTaste = new EcigarettesTaste
                        {
                            Name = taste
                        };
                    }

                    ecigaretteTasteMix.Tastes.Add(ecigaretteTaste);
                }

                _dbcontext.EcigaretteTasteMixes.Add(ecigaretteTasteMix);
                _dbcontext.SaveChanges();
            }

            return ecigaretteTasteMix.Id;
        }
    }
}
