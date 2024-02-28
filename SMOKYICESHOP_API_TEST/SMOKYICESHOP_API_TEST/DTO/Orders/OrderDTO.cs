using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.Orders
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public Guid? ClientID { get; set; }
        public string OrderStatus { get; set; } = null!;
        public string DeliveryMethod { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public string ClientFirstName { get; set; } = null!;
        public string ClientLastName { get; set; } = null!;

        public string? Comment { get; set; }
        public string? DeliveryRegion { get; set; }
        public string? DeliveryCity { get; set; }
        public string? PostOfficeNumber { get; set; }
        public string PhoneNumber { get; set; } = null!;

        public short TotalPrice { get; set; }
        public IEnumerable<OrderGoodDTO> Goods { get; set; } = null!;
    }
}
