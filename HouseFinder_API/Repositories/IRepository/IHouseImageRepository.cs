﻿using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IHouseImageRepository
    {
        public void CreateHouseImage(ImagesOfHouseDTO img, string LandlordId);
        public void UpdateHouseImage(ImagesOfHouseDTO img, string LandlordId);
    }
}
