using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class House
    {
        public House()
        {
            ImagesOfHouses = new HashSet<ImagesOfHouse>();
            Rates = new HashSet<Rate>();
            Reports = new HashSet<Report>();
            Rooms = new HashSet<Room>();
        }

        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public string Address { get; set; }
        public string GoogleMapLocation { get; set; }
        public string Information { get; set; }
        public int? VillageId { get; set; }
        public string LandlordId { get; set; }
        public int? CampusId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }

        public virtual Campus Campus { get; set; }
        public virtual User CreatedUserNavigation { get; set; }
        public virtual User Landlord { get; set; }
        public virtual User UpdatedUserNavigation { get; set; }
        public virtual Village Village { get; set; }
        public virtual ICollection<ImagesOfHouse> ImagesOfHouses { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
