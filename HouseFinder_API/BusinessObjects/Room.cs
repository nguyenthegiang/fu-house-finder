using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Room
    {
        public Room()
        {
            ImageOfRooms = new HashSet<ImageOfRoom>();
        }

        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public decimal? PricePerMonth { get; set; }
        public string Information { get; set; }
        public double? AreaByMeters { get; set; }
        public int? MaxAmountOfPeople { get; set; }
        public int? CurrentAmountOfPeople { get; set; }
        public int? BuildingNumber { get; set; }
        public int? FloorNumber { get; set; }
        public int? StatusId { get; set; }
        public int? RoomTypeId { get; set; }
        public int? HouseId { get; set; }

        public virtual House House { get; set; }
        public virtual RoomType RoomType { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<ImageOfRoom> ImageOfRooms { get; set; }
    }
}
