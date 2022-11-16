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
    public class CommuneDAO
    {
        //For HouseDAO.GetHouseById()
        public static CommuneDTO GetCommuneById(int CommuneId)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    return context.Communes.ProjectTo<CommuneDTO>(config)
                        .Where(commune => commune.CommuneId == CommuneId).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static int CountCommuneHavingHouse()
        {
            int total;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    total = context.Communes.Where(c => c.Villages.Where(v => v.Houses.Count() > 0).Count() > 0).Count();
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
