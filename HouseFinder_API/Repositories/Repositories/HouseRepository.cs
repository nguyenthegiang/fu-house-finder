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
        public List<HouseDTO> GetListHousesByLandlordId(string LandlordId) => HouseDAO.GetListHousesByLandlordId(LandlordId);

        public decimal? GetMoneyForNotRentedRooms(int HouseId) => HouseDAO.GetMoneyForNotRentedRooms(HouseId);

        public int CountTotalHouse() => HouseDAO.CountTotalHouse();

        public int CountAvailableHouse() => HouseDAO.CountAvailableHouse();
    }
}
