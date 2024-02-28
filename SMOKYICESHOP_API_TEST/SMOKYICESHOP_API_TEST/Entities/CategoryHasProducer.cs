using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class CategoryHasProducer
    {
        public string Category { get; set; } = null!;
        public Guid ProducerId { get; set; }
    }
}
