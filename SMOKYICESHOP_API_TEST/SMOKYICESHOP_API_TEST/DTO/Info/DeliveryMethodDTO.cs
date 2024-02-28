using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.Info
{
    public class DeliveryMethodDTO
    {
        public Guid Id { get; set; }
        public string NameCode { get; set; } = null!; 
        public string NameUkr { get; set; } = null!;
        public bool IsSelfDelivery { get; set; }
    }
}
