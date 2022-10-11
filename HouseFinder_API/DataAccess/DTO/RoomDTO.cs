﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class RoomDTO
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public decimal? PricePerMonth { get; set; }
        public string Information { get; set; }
        public double? AreaByMeters { get; set; }
        public bool Aircon { get; set; }
        public int? MaxAmountOfPeople { get; set; }
        public int? CurrentAmountOfPeople { get; set; }
        public int? BuildingNumber { get; set; }
        public int? FloorNumber { get; set; }
        public int? StatusId { get; set; }
        public int? RoomTypeId { get; set; }
        public int? HouseId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public virtual ICollection<ImagesOfRoomDTO> ImagesOfRooms { get; set; }
    }
}
