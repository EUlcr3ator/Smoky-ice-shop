using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class LiquidsGroup : IGroupEntity
    {
        public LiquidsGroup()
        {
            Liquids = new HashSet<Liquid>();
        }

        public Guid Id { get; set; }
        public string NicotineType { get; set; } = null!;
        public byte Capacity { get; set; }
        public string? Line { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProducerId { get; set; }
        public Guid ImageId { get; set; }

        public virtual Image Image { get; set; } = null!;
        public virtual Producer Producer { get; set; } = null!;
        public virtual ICollection<Liquid> Liquids { get; set; }
    }
}
