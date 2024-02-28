using SMOKYICESHOP_API_TEST.DTO.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.Cart
{
    public class CartGoodDTO : IDefaultGood
    {
        public Guid Id { get; set; }
        public Guid? ImageId { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public short Price { get; set; }
        public short? DiscountPrice { get; set; }
        public bool IsSold { get; set; }
        public Guid GroupId { get; set; }

        public int Count { get; set; }
    }
}
