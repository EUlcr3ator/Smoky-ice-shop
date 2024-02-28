using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.Orders
{
    public interface IDeliveryInfo
    {
        public Guid DeliveryID { get; set; }
        public Guid PaymentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Comment { get; set; }
        public string? DeliveryRegion { get; set; }
        public string? DeliveryCity { get; set; }
        public string? PostOfficeNumber { get; set; }
        public string PhoneNumber { get; set; }
    }
}
