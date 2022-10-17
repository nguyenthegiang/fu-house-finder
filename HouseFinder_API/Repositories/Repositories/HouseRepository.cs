using BusinessObjects;
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
    public class HouseRepository : IHousesRepository
    {
        public List<HouseDTO> GetAllHouses() => HouseDAO.GetAllHouses();
        public List<HouseDTO> GetHouseByName(string HouseName) => HouseDAO.GetHouseByName(HouseName);

        public HouseDTO GetHouseById(int HouseId) => HouseDAO.GetHouseById(HouseId);
        public int GetHouseCountByLandlordId(string landlordId) => HouseDAO.GetHouseCountByLandlordId(landlordId);
    }
}
