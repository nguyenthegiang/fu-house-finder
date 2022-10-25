using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObjects;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RoomTypeDAO
    {
        //[Home page] Get list RoomTypes for Filter
        public static List<RoomTypeDTO> GetRoomTypes()
        {
            List<RoomTypeDTO> roomTypes;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    roomTypes = context.RoomTypes.ProjectTo<RoomTypeDTO>(config).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return roomTypes;
        }

        //[List room] Get RoomTypes by HouseId: Get all roomTypes that this house has
        public static List<RoomTypeDTO> GetRoomTypesByHouseId(int houseId)
        {
            List<RoomTypeDTO> roomTypesOfHouse = new List<RoomTypeDTO>();
            List<RoomDTO> rooms = RoomDAO.GetRoomsByHouseId(houseId);

            try
            {
                //using (var context = new FUHouseFinderContext())
                //{
                //    //create Mapper
                //    MapperConfiguration config;
                //    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));

                //    //Get list of rooms by house id, so that can find their roomTypes
                //    rooms = context.Rooms.Where(r => r.HouseId == houseId).ToList();

                //    //Get list of room types for the rooms of this house
                //    foreach (Room room in rooms)
                //    {
                //        //first iteration: list roomType is empty -> add this roomType into the list
                //        if(roomTypesOfHouse.Count == 0)
                //        {
                //            //RoomTypeDTO result = context.RoomTypes.ProjectTo<RoomTypeDTO>(config).Where(roomTypeInList => roomTypeInList.RoomTypeId == room.RoomTypeId).FirstOrDefault();
                //            RoomTypeDTO result = allRoomTypes.Where(roomTypeInList => roomTypeInList.RoomTypeId == room.RoomTypeId).FirstOrDefault();
                //            roomTypesOfHouse.Add(result);
                //        }

                //        //which each of the other rooms:
                //        //check if its roomType has already been in the list; if not, add to list
                //        foreach (RoomTypeDTO roomType in roomTypesOfHouse)
                //        {
                //            if(room.RoomTypeId != roomType.RoomTypeId)
                //            {
                //                RoomTypeDTO result = allRoomTypes.Where(roomTypeInList => roomTypeInList.RoomTypeId == room.RoomTypeId).FirstOrDefault();
                //                roomTypesOfHouse.Add(result);
                //            }
                //        }
                //    }
                //}

                foreach(RoomDTO room in rooms)
                {
                    roomTypesOfHouse.Add(room.RoomType);
                }

                roomTypesOfHouse = roomTypesOfHouse.GroupBy(roomType => roomType.RoomTypeId).Select(roomType => roomType.First()).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return roomTypesOfHouse;
        }
    }
}
