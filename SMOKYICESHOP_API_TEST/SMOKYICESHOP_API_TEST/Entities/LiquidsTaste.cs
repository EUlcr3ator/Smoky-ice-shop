using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class LiquidsTaste
    {
        public LiquidsTaste()
        {
            TasteMixes = new HashSet<LiquidTasteMix>();
        }

        public Guid Id { get; set; }
        public string Taste { get; set; } = null!;
        public string TasteGroup { get; set; } = null!;

        public virtual ICollection<LiquidTasteMix> TasteMixes { get; set; }
    }
}
