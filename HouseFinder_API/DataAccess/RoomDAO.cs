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

        /**
         *  Get list Rooms by House
         *  Used in: RoomDAO.GetRoomPriceByOfHouse() [Home Page]
         Return list of Rooms of 1 House
         */
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
                    rooms = context.Rooms
                        //not getting deleted rooms
                        .Where(room => room.Deleted == false)
                        .Where(room => room.HouseId == HouseId)
                        .Include(room => room.ImagesOfRooms)
                        .Include(room => room.RoomType).ProjectTo<RoomDTO>(config).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return rooms;
        }

        /**
         *  [HouseDetail] Get list Available Rooms by House ID
         Get list of available Rooms (partially or totally) of 1 House
         */
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
                    var result = context.Rooms.Where(r => r.Deleted == false)
                        .Include(p => p.Status)
                        .Where(r => r.HouseId == HouseId && r.Deleted == false)
                        .ProjectTo<RoomDTO>(config).ToList();
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

        /**
         *  Get LowestRoomPrice & HighestRoomPrice for AvailableHouseDTO (used to display in Home Page)
            Used in: HouseDAO.GetAllHouses();
         */
        public static AvailableHouseDTO GetRoomPriceByOfHouse(AvailableHouseDTO houseDTO)
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
        public static RoomDTO GetRoomByHouseIdAndBuildingAndFloorAndRoomName(int HouseId, int Building, int Floor, string RoomName)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config = new MapperConfiguration(cfg => 
                        cfg.AddProfile(new MapperProfile())
                    );
                    RoomDTO room = context.Rooms.Where(r =>
                        r.HouseId == HouseId
                        && r.BuildingNumber == Building
                        && r.FloorNumber == Floor
                        && r.RoomName == RoomName
                    ).ProjectTo<RoomDTO>(config).FirstOrDefault();
                    return room;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /**
         Create room
         Add a new Room to the Database
         */
        public static RoomDTO CreateRoom(Room room)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    context.Rooms.Add(room);
                    context.SaveChanges();
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    var mapper = config.CreateMapper();
                    RoomDTO roomDTO = mapper.Map<RoomDTO>(room);
                    return roomDTO;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /**
         *  Create List of Rooms
            Add a list of Rooms of 1 House to the Database
         */
        public static void CreateRooms(List<Room> rooms)
        {
                using (var context = new FUHouseFinderContext())
                {
                    context.Rooms.AddRange(rooms);
                    context.SaveChanges();
                }
        }

        //Update Room by id
        public static void UpdateRoom(Room room)
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

        /**
         Delete a Room from a system without removing its data from the Database
         */
        public static void DeleteRoom(int roomId)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    Room updatedRoom = context.Rooms.FirstOrDefault(r => r.RoomId == roomId);
                    if (updatedRoom == null)
                    {
                        throw new Exception();
                    }

                    //Delete by changing Status to Disabled
                    updatedRoom.Deleted = true;
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

        /**
         Find a Room by its Id
         */
        public static RoomDTO GetRoomByRoomId(int roomId)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    RoomDTO room = context.Rooms.Where(r => r.Deleted == false)
                        .Include(p => p.Status)
                        .Where(r => r.RoomId == roomId && r.Deleted == false)
                        .ProjectTo<RoomDTO>(config).FirstOrDefault();
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

        /**
         *  [Staff - Dashboard] [Home Page] Count available rooms;
         Count the sum of all available Rooms (partially or totally) of all Houses in the system
         */
        public static int CountAvailableRoom()
        {
            int availableRoom = 0;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Count available rooms
                    availableRoom = context.Rooms
                        .Where(room => room.Deleted == false)
                        .Where(room => room.Status.StatusName.Equals("Available")).Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return availableRoom;
        }

        /**
         * [Staff - Dashboard] [Home Page] Count total rooms;
         Count the sum of all Rooms (available or not) of all Houses in the system
         */
        public static int CountTotalRoom()
        {
            int totalRoom = 0;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Count available rooms
                    totalRoom = context.Rooms.Where(r => r.Deleted == false).ToList().Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return totalRoom;
        }

        /**
         * [Staff - Dashboard] Count capacity of available rooms;
         Count the sum of Capacity of all Rooms of all Houses in the system
         */
        public static int? CountAvailableCapacity()
        {
            int? capacity;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get list of rooms
                    List<Room> rooms = context.Rooms.Where(r => r.Deleted == false).ToList();
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

        //[Staff/Dashboard] Count total capacity
        public static int? CountTotalCapacity()
        {
            int? capacity;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    List<RoomDTO> rooms = context.Rooms.Where(r => r.Deleted == false).ProjectTo<RoomDTO>(config).ToList();
                    capacity = (from room in rooms
                                select room.MaxAmountOfPeople).Sum();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return capacity;
        }

        //[Staff/Dashboard] Count total of totally available room
        public static int? CountTotallyAvailableRoom()
        {
            int? capacity;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    List<RoomDTO> rooms = context.Rooms.Where(r => r.Deleted == false).Where(r => r.CurrentAmountOfPeople == 0).ProjectTo<RoomDTO>(config).ToList();
                    capacity = (from room in rooms
                                select room.MaxAmountOfPeople).Sum();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return capacity;
        }

        /**
         * [Home Page] for HouseDAO.GetAvailableHouses()
         Count the sum of all available Rooms (partially or totally) of 1 House
         */
        public static int CountAvailableRoomByHouseId(int HouseId)
        {
            int countAvailableRoom = 0;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Count available rooms of 1 house
                    countAvailableRoom = context.Rooms
                        //not selecting deleted rooms
                        .Where(room => room.Deleted == false)
                        .Where(room => room.HouseId == HouseId)
                        .Where(room => room.Status.StatusName.Equals("Available"))
                        .Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return countAvailableRoom;
        }

        /**
         * [House Detail] Count total capacity by house id
         Count the sum of Capacity of all Rooms of 1 House
         */
        public static int? CountAvailableCapacityByHouseId(int HouseId)
        {
            int? capacity;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get list of available rooms
                    List<Room> rooms = context.Rooms.Where(r => r.Deleted == false).Where(r => r.HouseId == HouseId).Where(r => r.Status.StatusName.Equals("Available")).ToList();
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

        /**
         * [Homepage] Count totally available room by house id
         Count the sum of all available Rooms (totally only) of 1 House
         */
        public static int CountTotallyAvailableRoomByHouseId(int houseId)
        {
            int total;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get list totally available room by house id
                    List<RoomDTO> availableRooms = GetAvailableRoomsByHouseId(houseId).Where(r => r.CurrentAmountOfPeople == 0).ToList();
                    //Calculate the number of totally available rooms
                    total = availableRooms.Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return total;
        }

        /**
         * [Homepage] Count partially available room by house Id
         Count the sum of all available Rooms (partially only) of 1 House
         */
        public static int CountPartiallyAvailableRoomByHouseId(int houseId)
        {
            int total;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get list totally available room by house id
                    List<RoomDTO> availableRooms = GetAvailableRoomsByHouseId(houseId).Where(r => r.CurrentAmountOfPeople > 0).ToList();
                    //Calculate the number of partially available rooms
                    total = availableRooms.Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return total;
        }

        /**
         * [Landlord - List Room] Change status Room
         Change Status (Available or Occupied) of 1 Room
         */
        public static void ChangeStatusRoom(int statusId, int roomId)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    Room roomToUpdate = context.Rooms.FirstOrDefault(r => r.RoomId == roomId);
                    if(roomToUpdate == null)
                    {
                        throw new Exception();
                    }
                    roomToUpdate.LastModifiedDate = DateTime.Now;
                    roomToUpdate.StatusId = statusId;
                    context.Rooms.Update(roomToUpdate);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
