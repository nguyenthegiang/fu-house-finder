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
    public class StatusDAO
    {

        //[Landlord][List room] Get list Status for Filter
        public static List<StatusDTO> GetStatusesByHouseId(int houseId)
        {
            List<StatusDTO> statusesOfHouse = new List<StatusDTO>();
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
        public static List<StatusDTO> GetAllListStatus()
        {
            List<StatusDTO> statusDTOs = new List<StatusDTO>();
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get by Id, include Address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    statusDTOs = context.Statuses.ProjectTo<StatusDTO>(config).ToList();
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
