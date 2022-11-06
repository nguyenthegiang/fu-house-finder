using BusinessObjects;
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
        public HouseDTO CreateHouse(string houseName, string information, string _address, string ggAddress, string villageName, string landlordId, string campusName,
            decimal powerPrice, decimal waterPrice, bool fingerprintLock, bool camera, bool parking);
        //public List<HouseDTO> GetAllHouses();
        //public List<HouseDTO> GetHouseByName(string HouseName);
        public HouseDTO GetHouseById(int HouseId);
        public List<HouseDTO> GetListHousesByLandlordId(string LandlordId);
        public int CountTotalHouse();
        public decimal? GetMoneyForNotRentedRooms(int HouseId);
        public int CountAvailableHouse();
        public List<AvailableHouseDTO> GetAvailableHouses();
    }
}
