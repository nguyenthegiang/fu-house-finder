using System;
using System.Collections.Generic;

#nullable disable

namespace HouseFinder_API.Models
{
    public partial class Commune
    {
        public Commune()
        {
            Villages = new HashSet<Village>();
        }

        public int CommuneId { get; set; }
        public string CommunetName { get; set; }
        public int? DistrictId { get; set; }

        public virtual District District { get; set; }
        public virtual ICollection<Village> Villages { get; set; }
    }
}
