using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class Ecigarette : IHasGood
    {
        public Guid GoodId { get; set; }
        public Guid GroupId { get; set; }
        public Guid TasteMixId { get; set; }
        public byte Sweet { get; set; }
        public byte Sour { get; set; }
        public byte Fresh { get; set; }
        public byte Spicy { get; set; }

        public virtual Good Good { get; set; } = null!;
        public virtual EcigarettesGroup Group { get; set; } = null!;
        public virtual EcigaretteTasteMix TasteMix { get; set; } = null!;
    }
}
