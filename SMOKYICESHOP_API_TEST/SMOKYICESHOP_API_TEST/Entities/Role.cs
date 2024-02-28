using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class Role
    {
        public Role()
        {
            Clients = new HashSet<Client>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Client> Clients { get; set; }
    }
}
