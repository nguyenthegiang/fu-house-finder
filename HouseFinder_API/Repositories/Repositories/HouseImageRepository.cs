using DataAccess;
using DataAccess.DTO;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class HouseImageRepository : IHouseImageRepository
    {
        public void CreateHouseImage(ImagesOfHouseDTO img, string LandlordId) => ImageOfHouseDAO.CreateImageOfHouse(img.HouseId, img.ImageLink, LandlordId);
    }
}
