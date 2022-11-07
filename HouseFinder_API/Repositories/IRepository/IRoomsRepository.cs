﻿using BusinessObjects;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IRoomsRepository
    {
        public List<RoomDTO> GetRoomsByHouseId(int HouseId);
        public List<RoomDTO> GetAvailableRoomsByHouseId(int houseId);
        public void UpdateRoomByRoomId(Room room);
        public void CreateRoom(Room room);
        public void CreateRooms(List<Room> rooms);
        public void DeleteRoom(int roomId);
        public RoomDTO GetRoomByRoomId(int roomId);
        public int CountAvailableRoom();
        public int CountTotalRoom();
        public int CountAvailableRoomByHouseId(int houseId);
        public int? CountAvailableCapacity();
        public int? CountAvailableCapacityByHouseId(int houseId);

        public int CountTotallyAvailableRoomByHouseId(int houseId);
        public int CountPatiallyyAvailableRoomByHouseId(int houseId);
        public void ChangStatusRoom(int statusId, int roomId);
    }
}
