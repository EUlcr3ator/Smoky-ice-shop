using SMOKYICESHOP_API_TEST.DTO.Info;
using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class DeliveryMethodsModel
    {
        private readonly SmokyIceDbContext _dbcontext;

        public DeliveryMethodsModel(SmokyIceDbContext context)
        {
            _dbcontext = context;
        }

        public IEnumerable<DeliveryMethodDTO> GetDeliveryMethods()
        {
            return _dbcontext.DeliveryMethods
                .OrderBy(x => x.NameUkr)
                .Select(x => new DeliveryMethodDTO
                { 
                    Id = x.Id,
                    NameUkr = x.NameUkr,
                    NameCode = x.NameCode,
                    IsSelfDelivery = x.IsSelfDelivery
                })
                .ToList();
        }
    }
}
