using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects;

namespace HouseFinder_API.DTO
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<House, HouseDTO>();
        }
    }
}
