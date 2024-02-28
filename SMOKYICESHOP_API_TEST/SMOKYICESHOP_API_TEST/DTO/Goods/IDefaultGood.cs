using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.Goods
{
    public interface IDefaultGood : IGood
    {
        public Guid GroupId { get; set; }
    }
}
