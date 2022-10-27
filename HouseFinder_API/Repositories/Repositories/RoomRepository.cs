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
    public class RoomRepository : IRoomsRepository
    {
        public List<RoomDTO> GetAvailableRoomsByHouseId(int houseId) => RoomDAO.GetAvailableRoomsByHouseId(houseId);

        public List<RoomDTO> GetRoomsByHouseId(int HouseId) => RoomDAO.GetRoomsByHouseId(HouseId);

        public void UpdateRoomByRoomId(Room room) => RoomDAO.UpdateRoomByRoomId(room);

        public void CreateRoom(Room room) => RoomDAO.CreateRoom(room);

        public void DeleteRoom(int roomId) => RoomDAO.DeleteRoom(roomId);

        public RoomDTO GetRoomByRoomId(int roomId) => RoomDAO.GetRoomByRoomId(roomId);

        public int CountAvailableRoom() => RoomDAO.CountAvailableRoom();

        public int? CountAvailableCapacity() => RoomDAO.CountAvailableCapacity();
    }
}
