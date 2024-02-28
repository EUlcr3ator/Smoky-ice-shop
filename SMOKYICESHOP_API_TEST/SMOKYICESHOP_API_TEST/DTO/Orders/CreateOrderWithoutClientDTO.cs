using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.Orders
{
    public class CreateOrderWithoutClientDTO : IDeliveryInfo
    {
        public Guid DeliveryID { get; set; }
        public Guid PaymentID { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Comment { get; set; }
        public string? DeliveryRegion { get; set; }
        public string? DeliveryCity { get; set; }
        public string? PostOfficeNumber { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public IEnumerable<CreateOrderGoodDTO> Goods { get; set; } = null!;
    }
}
