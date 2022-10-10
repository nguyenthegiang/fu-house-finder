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
        public static List<HouseDTO> GetAllHouses()
        {
            List<HouseDTO> listHouses;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    listHouses = context.Houses.Include(h => h.Address).ProjectTo<HouseDTO>(config).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return listHouses;
        }
        public static List<HouseDTO> GetHouseByName(string name)
        {
            List<HouseDTO> listHouses;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    listHouses = context.Houses.Include(h => h.Address).ProjectTo<HouseDTO>(config).Where(p => p.HouseName.Contains(name)).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listHouses;
        }
    }
}
