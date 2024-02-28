using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class DeliveryInfo
    {
        public Guid Id { get; set; }
        public Guid DeliveryMethodId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? DeliveryRegion { get; set; }
        public string? DeliveryCity { get; set; }
        public string? PostOfficeNumber { get; set; }
        public string? Comment { get; set; }

        public virtual DeliveryMethod DeliveryMethod { get; set; } = null!;
        public virtual Order IdNavigation { get; set; } = null!;
        public virtual PaymentMethod PaymentMethod { get; set; } = null!;
    }
}
