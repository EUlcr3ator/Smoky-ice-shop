using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class HookahTobacco : IHasGood
    {
        public Guid GoodId { get; set; }
        public Guid GroupId { get; set; }
        public double Weight { get; set; }

        public virtual Good Good { get; set; } = null!;
        public virtual HookahTobaccoGroup Group { get; set; } = null!;
    }
}
