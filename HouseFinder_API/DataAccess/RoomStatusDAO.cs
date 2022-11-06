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
    public class RoomStatusDAO
    {

        //[Landlord][List room] Get list Status for Filter
        public static List<RoomStatusDTO> GetStatusesByHouseId(int houseId)
        {
            List<RoomStatusDTO> statusesOfHouse = new List<RoomStatusDTO>();
            List<RoomDTO> rooms = RoomDAO.GetRoomsByHouseId(houseId);
            try
            {
                foreach (RoomDTO room in rooms)
                {
                    statusesOfHouse.Add(room.Status);
                }

                statusesOfHouse = statusesOfHouse.GroupBy(status => status.StatusId).Select(status => status.First()).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return statusesOfHouse;
        }
        //Get All List Status
        public static List<RoomStatusDTO> GetAllListStatus()
        {
            List<RoomStatusDTO> statusDTOs = new List<RoomStatusDTO>();
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get by Id, include Address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    statusDTOs = context.RoomStatuses.ProjectTo<RoomStatusDTO>(config).ToList();
                    return statusDTOs;
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
