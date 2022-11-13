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
    public class VillageDAO
    {
        public static VillageDTO GetVillageByName(string name)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    return context.Villages.ProjectTo<VillageDTO>(config)
                        .Where(village => village.VillageName.Equals(name)).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //For HouseDAO.GetHouseById()
        public static VillageDTO GetVillageById(int VillageId)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    return context.Villages.ProjectTo<VillageDTO>(config)
                        .Where(village => village.VillageId == VillageId).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
