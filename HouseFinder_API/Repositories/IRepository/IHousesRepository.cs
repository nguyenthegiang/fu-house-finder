﻿using BusinessObjects;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IHousesRepository
    {
        public List<HouseDTO> GetAllHouses();
        public List<HouseDTO> GetHouseByName(string HouseName);
        public HouseDTO GetHouseById(int HouseId);
    }
}
