using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class EcigarettesTaste
    {
        public EcigarettesTaste()
        {
            Mixes = new HashSet<EcigaretteTasteMix>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<EcigaretteTasteMix> Mixes { get; set; }
    }
}
