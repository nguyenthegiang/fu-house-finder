using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Commune
    {
        public Commune()
        {
            Villages = new HashSet<Village>();
        }

        public int CommuneId { get; set; }
        public string CommuneName { get; set; }
        public int? DistrictId { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual District District { get; set; }
        public virtual ICollection<Village> Villages { get; set; }
    }
}
