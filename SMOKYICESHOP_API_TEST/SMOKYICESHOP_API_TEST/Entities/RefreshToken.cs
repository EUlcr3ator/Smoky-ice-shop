using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class RefreshToken
    {
        public Guid ClientId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }

        public virtual Client Client { get; set; } = null!;
    }
}
