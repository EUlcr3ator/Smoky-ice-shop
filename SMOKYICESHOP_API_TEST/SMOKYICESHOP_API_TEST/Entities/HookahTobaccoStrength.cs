using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class HookahTobaccoStrength
    {
        public HookahTobaccoStrength()
        {
            HookahTobaccoGroups = new HashSet<HookahTobaccoGroup>();
        }

        public byte Id { get; set; }
        public string Name { get; set; } = null!;
        public byte StrengthPoints { get; set; }

        public virtual ICollection<HookahTobaccoGroup> HookahTobaccoGroups { get; set; }
    }
}
