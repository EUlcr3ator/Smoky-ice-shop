using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class DiscountGood
    {
        public Guid GoodId { get; set; }
        public short DiscountPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual Good Good { get; set; } = null!;
    }
}
