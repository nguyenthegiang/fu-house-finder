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
    public class DistrictDAO
    {
        //(Unused) Get all Districts
        public static List<DistrictDTO> GetAllDistricts()
        {
            var districtDTOs = new List<DistrictDTO>();
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Map to DTO
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    //Get list of all Districts, with all its Communes, and which each Commune, all its Villages
                    districtDTOs = context.Districts.ProjectTo<DistrictDTO>(config).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return districtDTOs;
        }

        public static int CountDistrictHavingHouse()
        {
            int total;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    total = context.Districts.Where(d => d.Communes.Where(c => c.Villages.Where(v => v.Houses.Count() > 0).Count() > 0).Count() > 0).Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return total;
        }
    }
}
