using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.CreateGroups
{
    public class CreatePodGroupDTO
    {
        public double Weight { get; set; }
        public string Material { get; set; } = null!;
        public short Battarey { get; set; }
        public double CartrigeCapacity { get; set; }
        public double EvaporatorResistance { get; set; }
        public string Power { get; set; } = null!;
        public string Port { get; set; } = null!;
        public string? Line { get; set; }
        public string Name { get; set; } = null!;

        public Guid ImageId { get; set; }
        public Guid ProducerId { get; set; }
    }
}
