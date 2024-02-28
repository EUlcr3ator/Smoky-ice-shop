using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.CreateGroups
{
    public class CreateEcigaretteGroupDTO
    {
        public byte EvaporatorVolume { get; set; }
        public short BattareyCapacity { get; set; }
        public int PuffsCount { get; set; }
        public string? Line { get; set; }
        public string Name { get; set; } = null!;

        public Guid ImageId { get; set; }
        public Guid ProducerId { get; set; }
    }
}
