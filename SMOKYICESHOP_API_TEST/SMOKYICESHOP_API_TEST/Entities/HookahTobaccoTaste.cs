using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class HookahTobaccoTaste
    {
        public HookahTobaccoTaste()
        {
            Mixes = new HashSet<HookahTobaccoTasteMix>();
        }

        public Guid Id { get; set; }
        public string Taste { get; set; } = null!;

        public virtual ICollection<HookahTobaccoTasteMix> Mixes { get; set; }
    }
}
