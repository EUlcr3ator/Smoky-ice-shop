using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMOKYICESHOP_API_TEST.DTO.Authentication
{
    public class TelegramRegistrationDTO
    {
        public long UserID { get; set; }
        public string Phone { get; set; } = null!;
        public string TelegramKey { get; set; } = null!;
    }
}
