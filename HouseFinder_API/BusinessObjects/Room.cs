using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Room
    {
        public Room()
        {
            ImagesOfRooms = new HashSet<ImagesOfRoom>();
            RoomHistories = new HashSet<RoomHistory>();
        }

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

        public virtual User CreatedByNavigation { get; set; }
        public virtual House House { get; set; }
        public virtual User LastModifiedByNavigation { get; set; }
        public virtual RoomType RoomType { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<ImagesOfRoom> ImagesOfRooms { get; set; }
        public virtual ICollection<RoomHistory> RoomHistories { get; set; }
    }
}
