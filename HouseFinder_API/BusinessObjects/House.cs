using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class House
    {
        public House()
        {
            ImageOfHouses = new HashSet<ImageOfHouse>();
            Rates = new HashSet<Rate>();
            Rooms = new HashSet<Room>();
        }

        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public string Address { get; set; }
        public string GoogleMapLocation { get; set; }
        public string Information { get; set; }
        public int? VillageId { get; set; }
        public string LandlordId { get; set; }

        public virtual User Landlord { get; set; }
        public virtual Village Village { get; set; }
        public virtual ICollection<ImageOfHouse> ImageOfHouses { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
