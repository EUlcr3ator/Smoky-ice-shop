using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class HookahTobaccoTasteMix
    {
        public HookahTobaccoTasteMix()
        {
            HookahTobaccoGroups = new HashSet<HookahTobaccoGroup>();
            Tastes = new HashSet<HookahTobaccoTaste>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<HookahTobaccoGroup> HookahTobaccoGroups { get; set; }

        public virtual ICollection<HookahTobaccoTaste> Tastes { get; set; }
    }
}
