using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class HouseDTO
    {
        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public string Information { get; set; }
        public int AddressId { get; set; }
        public int? VillageId { get; set; }
        public string LandlordId { get; set; }
        public int? CampusId { get; set; }
        public decimal PowerPrice { get; set; }
        public decimal WaterPrice { get; set; }
        public bool? FingerprintLock { get; set; }
        public bool? Camera { get; set; }
        public bool? Parking { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        //Address of the House
        public virtual AddressDTO Address { get; set; }

        //List Images of this House
        public virtual ICollection<ImagesOfHouseDTO> ImagesOfHouses { get; set; }
    }
}
