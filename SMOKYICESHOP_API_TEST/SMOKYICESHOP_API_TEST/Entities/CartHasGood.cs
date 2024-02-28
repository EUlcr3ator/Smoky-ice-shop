using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class CartHasGood
    {
        public Guid GoodId { get; set; }
        public Guid CartId { get; set; }
        public byte ProductCount { get; set; }

        public virtual Client Cart { get; set; } = null!;
        public virtual Good Good { get; set; } = null!;
    }
}
