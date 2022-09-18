using System;
using System.Collections.Generic;

#nullable disable

namespace HouseFinder_API.Models
{
    public partial class District
    {
        public District()
        {
            Communes = new HashSet<Commune>();
        }

        public int DistrictId { get; set; }
        public string DistrictName { get; set; }

        public virtual ICollection<Commune> Communes { get; set; }
    }
}
