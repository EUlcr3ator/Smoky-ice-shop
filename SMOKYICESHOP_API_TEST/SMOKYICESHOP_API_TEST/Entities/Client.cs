using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class Client
    {
        public Client()
        {
            CartHasGoods = new HashSet<CartHasGood>();
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public long? ChatId { get; set; }
        public string? Password { get; set; }
        public string PhoneNumber { get; set; } = null!;

        public virtual Role Role { get; set; } = null!;
        public virtual RefreshToken RefreshToken { get; set; } = null!;
        public virtual ICollection<CartHasGood> CartHasGoods { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
