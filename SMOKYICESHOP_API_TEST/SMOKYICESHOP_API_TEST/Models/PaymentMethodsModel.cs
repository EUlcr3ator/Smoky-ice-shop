using SMOKYICESHOP_API_TEST.DTO.Info;
using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class PaymentMethodsModel
    {
        private readonly SmokyIceDbContext _dbcontext;

        public PaymentMethodsModel(SmokyIceDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IEnumerable<PaymentMethodDTO> GetPaymentMethods()
        {
            return _dbcontext.PaymentMethods
                .Select(x => new PaymentMethodDTO
                {   
                    Id = x.Id, 
                    NameCode = x.NameCode, 
                    NameUkr = x.NameUkr 
                })
                .ToList();
        }
    }
}
