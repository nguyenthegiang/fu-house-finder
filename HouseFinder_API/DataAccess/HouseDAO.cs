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
        private readonly FUHouseFinderContext context;
        private MapperConfiguration config;
        private IMapper mapper;

        public HouseDAO()
        {
            context = new FUHouseFinderContext();
            config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            mapper = config.CreateMapper();
        }
        public List<HouseDTO> GetAllHouses()
        {
            var listHouses = new List<HouseDTO>();
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    listHouses = context.Houses.Include(h => h.Address).ProjectTo<HouseDTO>(config).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return listHouses;
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
