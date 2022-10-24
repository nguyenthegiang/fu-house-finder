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
    }
}
