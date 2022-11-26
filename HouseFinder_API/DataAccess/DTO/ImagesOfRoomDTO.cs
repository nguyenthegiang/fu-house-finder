using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class ImagesOfRoomDTO
    {
        public int ImageId { get; set; }
        public string ImageLink { get; set; }
        public int RoomId { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
    }

    public class RoomImageInfoDTO
    {
        public int HouseId { get; set; }
        public string RoomName { get; set; }
        public int BuildingNumber { get; set; }
        public int FloorNumber { get; set; }
    }
}
