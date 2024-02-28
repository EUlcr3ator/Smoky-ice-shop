using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class Coal : IHasGood
    {
        public Guid GoodId { get; set; }
        public Guid GroupId { get; set; }
        public double Weight { get; set; }

        public virtual Good Good { get; set; } = null!;
        public virtual CoalsGroup Group { get; set; } = null!;
    }
}
