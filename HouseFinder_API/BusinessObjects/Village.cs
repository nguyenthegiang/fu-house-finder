using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Village
    {
        public Village()
        {
            Houses = new HashSet<House>();
        }

        public int VillageId { get; set; }
        public string VillageName { get; set; }
        public int? CommuneId { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Commune Commune { get; set; }
        public virtual ICollection<House> Houses { get; set; }
    }
}
