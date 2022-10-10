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
        private MapperConfiguration config;
        private IMapper mapper;

        public HouseDAO()
        {
            config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            mapper = config.CreateMapper();
        }
        public List<HouseDTO> GetAllHouses()
        {
            var listHouseDTOs = new List<HouseDTO>();
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    List<House> listHouse = context.Houses.ToList();
                    listHouseDTOs = listHouse.Select(m => mapper.Map<House, HouseDTO>(m)).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return listHouseDTOs;
        }
        public static List<House> GetHouseByName(string name)
        {
            var listHouses = new List<House>();
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    listHouses = context.Houses.Where(p => p.HouseName.Contains(name)).ToList();
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
