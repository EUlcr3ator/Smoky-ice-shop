using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class Pod : IHasGood
    {
        public Guid GoodId { get; set; }
        public Guid GroupId { get; set; }
        public string Appearance { get; set; } = null!;

        public virtual Good Good { get; set; } = null!;
        public virtual PodsGroup Group { get; set; } = null!;
    }
}
