using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class PodsGroup : IGroupEntity
    {
        public PodsGroup()
        {
            Pods = new HashSet<Pod>();
            Cartriges = new HashSet<CartrigesAndVaporizersGroup>();
        }

        public Guid Id { get; set; }
        public double Weight { get; set; }
        public string Material { get; set; } = null!;
        public short Battarey { get; set; }
        public double CartrigeCapacity { get; set; }
        public double EvaporatorResistance { get; set; }
        public string Power { get; set; } = null!;
        public string Port { get; set; } = null!;
        public string? Line { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProducerId { get; set; }
        public Guid ImageId { get; set; }

        public virtual Image Image { get; set; } = null!;
        public virtual Producer Producer { get; set; } = null!;
        public virtual ICollection<Pod> Pods { get; set; }

        public virtual ICollection<CartrigesAndVaporizersGroup> Cartriges { get; set; }
    }
}
