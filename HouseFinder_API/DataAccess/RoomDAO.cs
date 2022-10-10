using BusinessObjects;
using DataAccess.DTO;
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
        public static List<Room> GetRoomsByHouseId(int HouseId)
        {
            List<Room> rooms;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    rooms = context.Rooms.Where(r => r.HouseId == HouseId).ToList();
                }
            } catch (Exception e)
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
            List<Room> roomsOfHouse = GetRoomsByHouseId(houseDTO.HouseId);

            //Find LowestRoomPrice & HighestRoomPrice
            decimal lowestRoomPrice = decimal.MaxValue;
            decimal highestRoomPrice = decimal.MinValue;

            foreach (Room room in roomsOfHouse)
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
