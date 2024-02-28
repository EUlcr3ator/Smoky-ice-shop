using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class Category
    {
        public Category()
        {
            Goods = new HashSet<Good>();
            Producers = new HashSet<Producer>();
        }

        public byte Id { get; set; }
        public string Name { get; set; } = null!;
        public string NameUkr { get; set; } = null!;

        public virtual ICollection<Good> Goods { get; set; }

        public virtual ICollection<Producer> Producers { get; set; }
    }
}
