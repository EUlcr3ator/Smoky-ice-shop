using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.Info
{
    public class PaymentMethodDTO
    {
        public Guid Id { get; set; }
        public string NameCode { get; set; } = null!;
        public string NameUkr { get; set; } = null!;
    }
}
