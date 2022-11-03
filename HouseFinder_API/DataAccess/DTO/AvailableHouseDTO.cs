using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    /**
     DTO of House to display in Home Page, with different attributes
     */
    public class AvailableHouseDTO
    {
        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public string Information { get; set; }
        public int AddressId { get; set; }
        public int? VillageId { get; set; }

        //[Home Page - Filter by Region] Commune & District of this house
        public int? CommuneId { get; set; }
        public int? DistrictId { get; set; }

        public string LandlordId { get; set; }
        public int? CampusId { get; set; }
        public decimal PowerPrice { get; set; }
        public decimal WaterPrice { get; set; }
        public bool? FingerprintLock { get; set; }
        public bool? Camera { get; set; }
        public bool? Parking { get; set; }

        //[Home Page - Filter by RoomUtility] Utility that (at least 1) Rooms of this house has
        public bool Fridge { get; set; }
        public bool Kitchen { get; set; }
        public bool WashingMachine { get; set; }
        public bool Desk { get; set; }
        public bool NoLiveWithHost { get; set; }
        public bool Bed { get; set; }
        public bool ClosedToilet { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        //[Home Page] Price of the cheapest room & the most expensive room
        public decimal LowestRoomPrice { get; set; }
        public decimal HighestRoomPrice { get; set; }

        //Address of the House
        public virtual AddressDTO Address { get; set; }

        //[Home Page] Village of this House
        //for getting Data in DAO, will be deleted before sending through API
        public virtual VillageDTO Village { get; set; }

        //List Images of this House
        public virtual ICollection<ImagesOfHouseDTO> ImagesOfHouses { get; set; }

        //[Home Page] List Rooms of this House
        //for getting Data in DAO, will be deleted before sending through API
        public virtual ICollection<RoomDTO> Rooms { get; set; }

        //[Home Page - Filter by RoomType]
        //List RoomTypeIds (as a string) of all RoomTypes of all Rooms of this House
        public string RoomTypeIds { get; set; }
    }
}
