using System;
using System.Collections.Generic;

namespace SMOKYICESHOP_API_TEST.Entities
{
    public partial class EcigaretteTasteMix
    {
        public EcigaretteTasteMix()
        {
            Ecigarettes = new HashSet<Ecigarette>();
            Tastes = new HashSet<EcigarettesTaste>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Ecigarette> Ecigarettes { get; set; }

        public virtual ICollection<EcigarettesTaste> Tastes { get; set; }
    }
}
