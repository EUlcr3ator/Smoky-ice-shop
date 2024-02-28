using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DTO.Cart;
using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class ClientsModel
    {
        private readonly SmokyIceDbContext _dbcontext;

        public ClientsModel(SmokyIceDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public ClientDataDTO GetClientData(Guid clientId)
        {
            Order? order = _dbcontext.Orders
                .Where(x => x.ClientId == clientId)
                .OrderBy(x => x.CreationDate)
                .Include(x => x.DeliveryInfo)
                .FirstOrDefault();

            DeliveryInfo? info = order?.DeliveryInfo;

            if (info != null)
            {
                var data = new ClientDataDTO
                {
                    FirstName = info.FirstName,
                    LastName = info.LastName,
                    DeliveryRegion = info.DeliveryRegion,
                    DeliveryCity = info.DeliveryCity,
                    PostOfficeNumber = info.PostOfficeNumber,
                    DeliveryMethodId = info.DeliveryMethodId,
                    PaymentMethodId = info.PaymentMethodId
                };
                return data;
            }
            else
            {
                var data = new ClientDataDTO
                {
                    FirstName = "",
                    LastName = "",
                    DeliveryRegion = "",
                    DeliveryCity = "",
                    PostOfficeNumber = "",
                    DeliveryMethodId = Guid.Empty,
                    PaymentMethodId = Guid.Empty
                };
                return data;
            }
            

            
        }
    }
}
