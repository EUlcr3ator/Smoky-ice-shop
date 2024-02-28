using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class CoalsGroup : IGroupEntity
    {
        public CoalsGroup()
        {
            Coals = new HashSet<Coal>();
        }

        public Guid Id { get; set; }
        public string Type { get; set; } = null!;
        public double CubeSize { get; set; }
        public string? Line { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProducerId { get; set; }
        public Guid ImageId { get; set; }

        public virtual Image Image { get; set; } = null!;
        public virtual Producer Producer { get; set; } = null!;
        public virtual ICollection<Coal> Coals { get; set; }
    }
}
