using System;
using System.Collections.Generic;

#nullable disable

namespace HouseFinder_API.Models
{
    public partial class AdministrativeUnit
    {
        public AdministrativeUnit()
        {
            Houses = new HashSet<House>();
        }

        public int UnitCode { get; set; }
        public string UnitName { get; set; }
        public int? UnitLevel { get; set; }
        public int? ParentCode { get; set; }

        public virtual ICollection<House> Houses { get; set; }
    }
}
