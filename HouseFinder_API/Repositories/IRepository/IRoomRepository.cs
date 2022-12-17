using BusinessObjects;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IRoomRepository
    {
        public List<RoomDTO> GetRoomsByHouseId(int HouseId);
        public List<RoomDTO> GetAvailableRoomsByHouseId(int houseId);
        public void UpdateRoom(Room room);
        public RoomDTO CreateRoom(Room room);
        public void CreateRooms(List<Room> rooms);
        public void DeleteRoom(int roomId);
        public RoomDTO GetRoomByRoomId(int roomId);
        public RoomDTO GetRoomByHouseIdAndBuildingAndFloorAndRoomName(int HouseId, int Building, int Floor, string RoomName);
        public int CountAvailableRoom();
        public int CountTotalRoom();
        public int CountAvailableRoomByHouseId(int houseId);
        public int? CountAvailableCapacity();
        public int? CountTotalCapacity();
        public int? CountTotallyAvailableRoom();
        public int? CountAvailableCapacityByHouseId(int houseId);

        public int CountTotallyAvailableRoomByHouseId(int houseId);
        public int CountPatiallyyAvailableRoomByHouseId(int houseId);
        public void ChangeStatusRoom(int statusId, int roomId);
    }
}
