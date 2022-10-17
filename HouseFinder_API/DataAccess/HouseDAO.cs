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
    public class HouseDAO
    {
        //Get list of houses, with Address
        public static List<HouseDTO> GetAllHouses()
        {
            List<HouseDTO> houseDTOs;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //include address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    houseDTOs = context.Houses.Include(h => h.Address).ProjectTo<HouseDTO>(config).ToList();

                    //find lowest room price & highest room price
                    for (int i = 0; i < houseDTOs.Count; i++)
                    {
                        houseDTOs[i] = RoomDAO.GetRoomPriceForHouseDTO(houseDTOs[i]);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return houseDTOs;
        }

        //Search house by name, with Address
        public static List<HouseDTO> GetHouseByName(string houseName)
        {
            List<HouseDTO> houseDTOs;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //include Address into Houses
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    houseDTOs = context.Houses.Include(h => h.Address).ProjectTo<HouseDTO>(config).Where(p => p.HouseName.Contains(houseName)).ToList();

                    //find lowest room price & highest room price
                    for (int i = 0; i < houseDTOs.Count; i++)
                    {
                        houseDTOs[i] = RoomDAO.GetRoomPriceForHouseDTO(houseDTOs[i]);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return houseDTOs;
        }

        //[House Detail] Get House Detail information
        public static HouseDTO GetHouseById(int houseId)          
        {
            HouseDTO houseDTO;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //include Address into Houses
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    //Get by ID
                    houseDTO = context.Houses.Include(h => h.Address).ProjectTo<HouseDTO>(config)
                        .Where(p => p.HouseId == houseId).FirstOrDefault();

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return houseDTO;
        }
        //Get House Count By User id
        public static int GetHouseCountByLandlordId(string landlordId)
        {
            int count;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //include Address into Houses
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    //Get by ID
                    count = context.Houses.Where(p => p.LandlordId == landlordId).ToList().Count;

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return count;
        }
    }
}
