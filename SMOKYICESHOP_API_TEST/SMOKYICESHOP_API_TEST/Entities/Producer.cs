using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class Producer
    {
        public Producer()
        {
            CartrigesAndVaporizersGroups = new HashSet<CartrigesAndVaporizersGroup>();
            CoalsGroups = new HashSet<CoalsGroup>();
            EcigarettesGroups = new HashSet<EcigarettesGroup>();
            HookahTobaccoGroups = new HashSet<HookahTobaccoGroup>();
            LiquidsGroups = new HashSet<LiquidsGroup>();
            PodsGroups = new HashSet<PodsGroup>();
            Categories = new HashSet<Category>();
        }

        public Guid Id { get; set; }
        public Guid ImageId { get; set; }
        public string Name { get; set; } = null!;

        public virtual Image Image { get; set; } = null!;
        public virtual ICollection<CartrigesAndVaporizersGroup> CartrigesAndVaporizersGroups { get; set; }
        public virtual ICollection<CoalsGroup> CoalsGroups { get; set; }
        public virtual ICollection<EcigarettesGroup> EcigarettesGroups { get; set; }
        public virtual ICollection<HookahTobaccoGroup> HookahTobaccoGroups { get; set; }
        public virtual ICollection<LiquidsGroup> LiquidsGroups { get; set; }
        public virtual ICollection<PodsGroup> PodsGroups { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
