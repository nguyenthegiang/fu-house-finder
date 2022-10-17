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
            } catch (Exception e)
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
                using(var context = new FUHouseFinderContext())
                {
                    //Find rooms of this house
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    var result = context.Rooms.Include(p => p.Status).Where(r => r.HouseId == HouseId).ProjectTo<RoomDTO>(config).ToList();
                    //Get only available rooms
                    foreach (RoomDTO r in result)
                    {
                        if(r.Status.StatusName.Equals("Available"))
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
    }
}
