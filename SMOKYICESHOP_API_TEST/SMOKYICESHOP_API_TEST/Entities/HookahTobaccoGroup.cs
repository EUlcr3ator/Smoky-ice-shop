using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class HookahTobaccoGroup : IGroupEntity
    {
        public HookahTobaccoGroup()
        {
            HookahTobaccos = new HashSet<HookahTobacco>();
        }

        public Guid Id { get; set; }
        public Guid TasteMixId { get; set; }
        public byte Sweet { get; set; }
        public byte Sour { get; set; }
        public byte Spicy { get; set; }
        public byte Fresh { get; set; }
        public byte StrengthId { get; set; }
        public string? Line { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProducerId { get; set; }
        public Guid ImageId { get; set; }

        public virtual Image Image { get; set; } = null!;
        public virtual Producer Producer { get; set; } = null!;
        public virtual HookahTobaccoStrength Strength { get; set; } = null!;
        public virtual HookahTobaccoTasteMix TasteMix { get; set; } = null!;
        public virtual ICollection<HookahTobacco> HookahTobaccos { get; set; }
    }
}
