using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class CartrigesAndVaporizer : IHasGood
    {
        public Guid GoodId { get; set; }
        public Guid GroupId { get; set; }
        public double Resistance { get; set; }

        public virtual Good Good { get; set; } = null!;
        public virtual CartrigesAndVaporizersGroup Group { get; set; } = null!;
    }
}
