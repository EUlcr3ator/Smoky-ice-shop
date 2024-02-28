using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.CreateGoods
{
    public class CreateCoalGoodDTO : ICreateGood
    {
        public Guid GroupId { get; set; }
        public Guid ImageId { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public short Price { get; set; }
        public bool IsSold { get; set; }

        public double Weight { get; set; }
    }
}
