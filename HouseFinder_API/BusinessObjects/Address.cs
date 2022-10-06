using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Address
    {
        public Address()
        {
            Campuses = new HashSet<Campus>();
            Houses = new HashSet<House>();
            Users = new HashSet<User>();
        }

        public int AddressId { get; set; }
        public string Addresses { get; set; }
        public string GoogleMapLocation { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<Campus> Campuses { get; set; }
        public virtual ICollection<House> Houses { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
