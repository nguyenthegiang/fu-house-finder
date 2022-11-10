using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Campus
    {
        public Campus()
        {
            Districts = new HashSet<District>();
            Houses = new HashSet<House>();
        }

        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public int AddressId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<House> Houses { get; set; }
    }
}
