using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Campus
    {
        public Campus()
        {
            Houses = new HashSet<House>();
        }

        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public string GoogleMapLocation { get; set; }

        public virtual ICollection<House> Houses { get; set; }
    }
}
