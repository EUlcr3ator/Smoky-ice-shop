using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class CartrigesAndVaporizersGroup : IGroupEntity
    {
        public CartrigesAndVaporizersGroup()
        {
            CartrigesAndVaporizers = new HashSet<CartrigesAndVaporizer>();
            Pods = new HashSet<PodsGroup>();
        }

        public Guid Id { get; set; }
        public double? CartrigeCapacity { get; set; }
        public string? SpiralType { get; set; }
        public bool IsVaporizer { get; set; }
        public string? Line { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProducerId { get; set; }
        public Guid ImageId { get; set; }

        public virtual Image Image { get; set; } = null!;
        public virtual Producer Producer { get; set; } = null!;
        public virtual ICollection<CartrigesAndVaporizer> CartrigesAndVaporizers { get; set; }

        public virtual ICollection<PodsGroup> Pods { get; set; }
    }
}
