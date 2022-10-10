﻿using System;
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
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        //Price of the cheapest room & the most expensive room, to display in Home Page
        public decimal LowestRoomPrice { get; set; }
        public decimal HighestRoomPrice { get; set; }

        //Address of the House
        public virtual AddressDTO Address { get; set; }

    }
}
