using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class HouseDTO
    {
        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public int? View { get; set; }
        public string Information { get; set; }
        public int AddressId { get; set; }
        public int? VillageId { get; set; }
        //Additional information for Display in [Update House] & [House Detail]
        public int? CommuneId { get; set; }
        public int? DistrictId { get; set; }
        public string LandlordId { get; set; }
        public int? CampusId { get; set; }
        public double? DistanceToCampus { get; set; }
        public decimal PowerPrice { get; set; }
        public decimal WaterPrice { get; set; }
        public bool? FingerprintLock { get; set; }
        public bool? Camera { get; set; }
        public bool? Parking { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        //Address of the House
        public virtual AddressDTO Address { get; set; }

        //List Images of this House
        public virtual ICollection<ImagesOfHouseDTO> ImagesOfHouses { get; set; }
    }

    public class CreateHouseDTO
    {
        public string HouseName { get; set; }
        public string Information { get; set; }
        public string Address { get; set; }
        public string GoogleAddress { get; set; }
        public int VillageId { get; set; }
        public string LandlordId { get; set; }
        public int CampusId { get; set; }
        public double DistanceToCampus { get; set; }
        public decimal PowerPrice { get; set; }
        public decimal WaterPrice { get; set; }
        public bool FingerprintLock { get; set; }
        public bool Camera { get; set; }
        public bool Parking { get; set; }
    }

    public class UpdateHouseDTO
    {
        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public string Information { get; set; }
        public string Address { get; set; }
        public int VillageId { get; set; }
        public int CampusId { get; set; }
        public decimal PowerPrice { get; set; }
        public decimal WaterPrice { get; set; }
        public bool FingerprintLock { get; set; }
        public bool Camera { get; set; }
        public bool Parking { get; set; }
        [JsonIgnore]
        public string ModifiedBy { get; set; }
    }
}
