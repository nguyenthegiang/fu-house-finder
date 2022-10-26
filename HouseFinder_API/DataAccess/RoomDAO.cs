using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObjects;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RoomDAO
    {
        //Get list Rooms by House
        public static List<RoomDTO> GetRoomsByHouseId(int HouseId)
        {
            List<RoomDTO> rooms;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get by HouseID, include Images
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    rooms = context.Rooms.Where(r => r.HouseId == HouseId)
                        .Include(r => r.ImagesOfRooms).Include(r => r.RoomType).ProjectTo<RoomDTO>(config).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return rooms;
        }

        //[HouseDetail] Get list Available Rooms by House ID
        public static List<RoomDTO> GetAvailableRoomsByHouseId(int HouseId)
        {
            List<RoomDTO> rooms = new List<RoomDTO>();
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Find rooms of this house
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    var result = context.Rooms.Include(p => p.Status).Where(r => r.HouseId == HouseId).ProjectTo<RoomDTO>(config).ToList();
                    //Get only available rooms
                    foreach (RoomDTO r in result)
                    {
                        if (r.Status.StatusName.Equals("Available"))
                        {
                            rooms.Add(r);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return rooms;
        }

        //Get LowestRoomPrice & LowestRoomPrice for HouseDTO (used to display in Home Page)
        //Used in: HouseDAO.GetAllHouses();
        public static HouseDTO GetRoomPriceForHouseDTO(HouseDTO houseDTO)
        {
            //Get list of its room
            List<RoomDTO> roomsOfHouse = GetRoomsByHouseId(houseDTO.HouseId);

            //Find LowestRoomPrice & HighestRoomPrice
            decimal lowestRoomPrice = decimal.MaxValue;
            decimal highestRoomPrice = decimal.MinValue;

            foreach (RoomDTO room in roomsOfHouse)
            {
                if (room.PricePerMonth > highestRoomPrice)
                {
                    highestRoomPrice = (decimal)room.PricePerMonth;
                }
                if (room.PricePerMonth < lowestRoomPrice)
                {
                    lowestRoomPrice = (decimal)room.PricePerMonth;
                }
            }

            //Assign values to HouseDTO
            houseDTO.LowestRoomPrice = lowestRoomPrice;
            houseDTO.HighestRoomPrice = highestRoomPrice;

            return houseDTO;
        }

        //Create room
        public static void CreateRoom(Room room)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    context.Rooms.Add(room);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //Update Room by id
        public static void UpdateRoomByRoomId(Room room)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Find rooms of this house
                    Room updatedRoom = context.Rooms.FirstOrDefault(r => r.RoomId == room.RoomId);
                    if (updatedRoom == null)
                    {
                        throw new Exception();
                    }

                    //Update
                    context.Entry<Room>(updatedRoom).State = EntityState.Detached;
                    context.Rooms.Update(room);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void DeleteRoom(int roomId)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    Room updatedRoom = context.Rooms.FirstOrDefault(r => r.RoomId == roomId);
                    if(updatedRoom == null)
                    {
                        throw new Exception();
                    }

                    //Delete by changing Status to Disabled
                    updatedRoom.StatusId = 3;
                    context.Entry<Room>(updatedRoom).State = EntityState.Detached;
                    context.Rooms.Update(updatedRoom);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static RoomDTO GetRoomByRoomId(int roomId)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    RoomDTO room = context.Rooms.Include(p => p.Status).Where(r => r.RoomId == roomId).ProjectTo<RoomDTO>(config).FirstOrDefault();
                    if (room == null)
                    {
                        throw new Exception();
                    }
                    return room;
                }
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //[Staff - Dashboard] Count available rooms
        public static int CountAvailableRoom()
        {
            int availableRoom = 0;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Count available rooms
                    availableRoom = context.Rooms.Where(r => r.Status.StatusName.Equals("Available")).Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return availableRoom;
        }

        //[Staff - Dashboard] Count capacity of available rooms
        public static int? CountAvailableCapacity()
        {
            int? capacity;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get list of rooms
                    List<Room> rooms = context.Rooms.ToList();
                    //Calculate
                    int? maxPeople = (from r in rooms select r.MaxAmountOfPeople).Sum();
                    int? currentPeople = (from r in rooms select r.CurrentAmountOfPeople).Sum();
                    capacity = maxPeople - currentPeople;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return capacity;
        }
    }
}
