using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.Cart
{
    public class ClientDataDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? DeliveryRegion { get; set; }
        public string? DeliveryCity { get; set; }
        public string? PostOfficeNumber { get; set; }

        public Guid DeliveryMethodId { get; set; }
        public Guid PaymentMethodId { get; set; }
    }
}
