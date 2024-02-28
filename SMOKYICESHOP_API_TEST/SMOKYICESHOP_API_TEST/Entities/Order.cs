using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderHasGoods = new HashSet<OrderHasGood>();
        }

        public Guid Id { get; set; }
        public Guid? ClientId { get; set; }
        public Guid OrderStatusId { get; set; }
        public short TotalPrice { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Client? Client { get; set; }
        public virtual OrderStatus OrderStatus { get; set; } = null!;
        public virtual DeliveryInfo DeliveryInfo { get; set; } = null!;
        public virtual ICollection<OrderHasGood> OrderHasGoods { get; set; }
    }
}
