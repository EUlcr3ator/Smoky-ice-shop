using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class EcigarettesGroup : IGroupEntity
    {
        public EcigarettesGroup()
        {
            Ecigarettes = new HashSet<Ecigarette>();
        }

        public Guid Id { get; set; }
        public byte EvaporatorVolume { get; set; }
        public short BatteryCapacity { get; set; }
        public int PuffsCount { get; set; }
        public string? Line { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProducerId { get; set; }
        public Guid ImageId { get; set; }

        public virtual Image Image { get; set; } = null!;
        public virtual Producer Producer { get; set; } = null!;
        public virtual ICollection<Ecigarette> Ecigarettes { get; set; }
    }
}
