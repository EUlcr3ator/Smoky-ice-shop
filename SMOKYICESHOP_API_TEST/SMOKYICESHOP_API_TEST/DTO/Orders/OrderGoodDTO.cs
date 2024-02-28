using SMOKYICESHOP_API_TEST.DTO.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.Orders
{
    public class OrderGoodDTO
    {
        public DefaultGoodDTO Good { get; set; } = null!;
        public short Count { get; set; }
        public short Price { get; set; }
    }
}
