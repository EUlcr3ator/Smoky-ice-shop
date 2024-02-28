using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class Good
    {
        public Good()
        {
            CartHasGoods = new HashSet<CartHasGood>();
            OrderHasGoods = new HashSet<OrderHasGood>();
        }

        public Guid Id { get; set; }
        public Guid ImageId { get; set; }
        public string Name { get; set; } = null!;
        public byte CategoryId { get; set; }
        public short Price { get; set; }
        public bool IsSold { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Image Image { get; set; } = null!;
        public virtual CartrigesAndVaporizer CartrigesAndVaporizer { get; set; } = null!;
        public virtual Coal Coal { get; set; } = null!;
        public virtual DiscountGood DiscountGood { get; set; } = null!;
        public virtual Ecigarette Ecigarette { get; set; } = null!;
        public virtual HookahTobacco HookahTobacco { get; set; } = null!;
        public virtual Liquid Liquid { get; set; } = null!;
        public virtual Pod Pod { get; set; } = null!;
        public virtual PopularGood PopularGood { get; set; } = null!;
        public virtual RecomendedGood RecomendedGood { get; set; } = null!;
        public virtual ICollection<CartHasGood> CartHasGoods { get; set; }
        public virtual ICollection<OrderHasGood> OrderHasGoods { get; set; }
    }
}
