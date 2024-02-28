using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class LiquidTasteMix
    {
        public LiquidTasteMix()
        {
            Liquids = new HashSet<Liquid>();
            Tastes = new HashSet<LiquidsTaste>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Liquid> Liquids { get; set; }

        public virtual ICollection<LiquidsTaste> Tastes { get; set; }
    }
}
