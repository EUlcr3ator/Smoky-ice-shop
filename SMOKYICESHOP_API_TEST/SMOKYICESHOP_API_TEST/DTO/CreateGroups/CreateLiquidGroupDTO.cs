using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.CreateGroups
{
    public class CreateLiquidGroupDTO
    {
        public string NicotineType { get; set; } = null!;
        public byte Capacity { get; set; }
        public string? Line { get; set; }
        public string Name { get; set; } = null!;

        public Guid ImageId { get; set; }
        public Guid ProducerId { get; set; }
    }
}
