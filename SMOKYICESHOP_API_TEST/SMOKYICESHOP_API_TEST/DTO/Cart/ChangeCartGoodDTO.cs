using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.Cart
{
    public class ChangeCartGoodDTO
    {
        public Guid GoodID { get; set; }
        public byte Count { get; set; }
    }
}
