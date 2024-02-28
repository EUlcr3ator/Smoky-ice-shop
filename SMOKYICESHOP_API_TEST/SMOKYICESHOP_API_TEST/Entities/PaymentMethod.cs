using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            DeliveryInfos = new HashSet<DeliveryInfo>();
        }

        public Guid Id { get; set; }
        public string NameCode { get; set; } = null!;
        public string NameUkr { get; set; } = null!;

        public virtual ICollection<DeliveryInfo> DeliveryInfos { get; set; }
    }
}
