using BusinessObjects;
using DataAccess;
using DataAccess.DTO;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        public List<RoomDTO> GetAvailableRoomsByHouseId(int houseId) => RoomDAO.GetAvailableRoomsByHouseId(houseId);

        public List<RoomDTO> GetRoomsByHouseId(int HouseId) => RoomDAO.GetRoomsByHouseId(HouseId);

        public void UpdateRoomByRoomId(Room room) => RoomDAO.UpdateRoomByRoomId(room);

        public void CreateRoom(Room room) => RoomDAO.CreateRoom(room);
        public void CreateRooms(List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                room.Deleted = false;
                room.CreatedDate = DateTime.UtcNow;
                room.LastModifiedDate = DateTime.UtcNow;
            }
            RoomDAO.CreateRooms(rooms);
        }

        public void DeleteRoom(int roomId) => RoomDAO.DeleteRoom(roomId);

        public RoomDTO GetRoomByRoomId(int roomId) => RoomDAO.GetRoomByRoomId(roomId);

        public int CountAvailableRoom() => RoomDAO.CountAvailableRoom();

        public int? CountAvailableCapacity() => RoomDAO.CountAvailableCapacity();

        public int CountTotallyAvailableRoomByHouseId(int houseId) => RoomDAO.CountTotallyAvailableRoomByHouseId(houseId);

        public int CountPatiallyyAvailableRoomByHouseId(int houseId) => RoomDAO.CountPartiallyAvailableRoomByHouseId(houseId);

        public int CountAvailableRoomByHouseId(int houseId) => RoomDAO.CountAvailableRoomByHouseId(houseId);

        public int? CountAvailableCapacityByHouseId(int houseId) => RoomDAO.CountAvailableCapacityByHouseId(houseId);
        
        public void ChangStatusRoom(int statusId, int roomId) => RoomDAO.ChangStatusRoom(statusId, roomId);

        public int CountTotalRoom() => RoomDAO.CountTotalRoom();
    }
}
