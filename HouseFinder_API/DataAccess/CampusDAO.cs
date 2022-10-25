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
    public class CampusDAO
    {
        //[Home Page] Get List Campuses to choose to filter
        public static List<CampusDTO> GetAllCampuses()
        {
            var campusDTOs = new List<CampusDTO>();
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Map to DTO
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    //Get list of all Campuses
                    campusDTOs = context.Campuses.ProjectTo<CampusDTO>(config).ToList();
                }
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return campusDTOs;
        }
    }
}
