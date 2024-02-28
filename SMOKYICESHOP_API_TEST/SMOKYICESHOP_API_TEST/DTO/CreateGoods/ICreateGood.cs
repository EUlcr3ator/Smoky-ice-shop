using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.CreateGoods
{
    public interface ICreateGood
    {
        public Guid GroupId { get; set; }
        public Guid ImageId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public short Price { get; set; }
        public bool IsSold { get; set; }
    }
}
