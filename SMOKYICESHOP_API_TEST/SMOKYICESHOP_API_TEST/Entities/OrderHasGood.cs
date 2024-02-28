using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class OrderHasGood
    {
        public Guid GoodId { get; set; }
        public Guid OrderId { get; set; }
        public byte ProductCount { get; set; }
        public short Price { get; set; }

        public virtual Good Good { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
