using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class PopularGood
    {
        public Guid GoodId { get; set; }

        public virtual Good Good { get; set; } = null!;
    }
}
