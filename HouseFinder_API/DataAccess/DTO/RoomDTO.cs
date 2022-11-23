using System;
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
        public bool Fridge { get; set; }
        public bool Kitchen { get; set; }
        public bool WashingMachine { get; set; }
        public bool Desk { get; set; }
        public bool NoLiveWithHost { get; set; }
        public bool Bed { get; set; }
        public bool ClosedToilet { get; set; }
        public int? MaxAmountOfPeople { get; set; }
        public int? CurrentAmountOfPeople { get; set; }
        public int? BuildingNumber { get; set; }
        public int? FloorNumber { get; set; }
        public int StatusId { get; set; }
        public virtual RoomStatusDTO Status { get; set; }
        public virtual RoomTypeDTO RoomType { get; set; }
        public int? HouseId { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        //List Image of this Room
        public virtual ICollection<ImagesOfRoomDTO> ImagesOfRooms { get; set; }

        //RoomTypeId, for Home Page - Filter available Rooms
        public int? RoomTypeId { get; set; }
    }

    public class RoomImageInfoDTO
    {
        public int HouseId { get; set; }
        public int RoomId { get; set; }
    }
}
