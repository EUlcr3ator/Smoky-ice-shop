using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class Image
    {
        public Image()
        {
            CartrigesAndVaporizersGroups = new HashSet<CartrigesAndVaporizersGroup>();
            CoalsGroups = new HashSet<CoalsGroup>();
            EcigarettesGroups = new HashSet<EcigarettesGroup>();
            Goods = new HashSet<Good>();
            HookahTobaccoGroups = new HashSet<HookahTobaccoGroup>();
            LiquidsGroups = new HashSet<LiquidsGroup>();
            PodsGroups = new HashSet<PodsGroup>();
            Producers = new HashSet<Producer>();
        }

        public Guid Id { get; set; }
        public byte[] ImageBinary { get; set; } = null!;

        public virtual ICollection<CartrigesAndVaporizersGroup> CartrigesAndVaporizersGroups { get; set; }
        public virtual ICollection<CoalsGroup> CoalsGroups { get; set; }
        public virtual ICollection<EcigarettesGroup> EcigarettesGroups { get; set; }
        public virtual ICollection<Good> Goods { get; set; }
        public virtual ICollection<HookahTobaccoGroup> HookahTobaccoGroups { get; set; }
        public virtual ICollection<LiquidsGroup> LiquidsGroups { get; set; }
        public virtual ICollection<PodsGroup> PodsGroups { get; set; }
        public virtual ICollection<Producer> Producers { get; set; }
    }
}
