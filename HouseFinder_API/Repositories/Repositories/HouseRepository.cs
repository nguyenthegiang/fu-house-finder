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
    public class HouseRepository : IHouseRepository
    {
        public HouseDTO CreateHouse(string houseName, string information, string _address, string ggAddress, string villageName, string landlordId, string campusName,
            decimal powerPrice, decimal waterPrice, bool fingerprintLock, bool camera, bool parking)
        {
            AddressDTO address = AddressDAO.CreateAddress(_address, ggAddress);
            VillageDTO village = VillageDAO.GetVillageByName(villageName);
            CampusDTO campus = CampusDAO.GetCampusByName(campusName);
            HouseDTO house = HouseDAO.CreateHouse(houseName, information, address.AddressId, village.VillageId, landlordId, campus.CampusId,
                powerPrice, waterPrice, fingerprintLock, camera, parking);
            return house;
        }
        //public List<HouseDTO> GetAllHouses() => HouseDAO.GetAllHouses();
        //public List<HouseDTO> GetHouseByName(string HouseName) => HouseDAO.GetHouseByName(HouseName);
        public HouseDTO GetHouseById(int HouseId) => HouseDAO.GetHouseById(HouseId);
        public List<HouseDTO> GetListHousesByLandlordId(string LandlordId) => HouseDAO.GetListHousesByLandlordId(LandlordId);

        public decimal? GetMoneyForNotRentedRooms(int HouseId) => HouseDAO.GetMoneyForNotRentedRooms(HouseId);

        public int CountTotalHouse() => HouseDAO.CountTotalHouse();

        public int CountAvailableHouse() => HouseDAO.CountAvailableHouse();
        public List<AvailableHouseDTO> GetAvailableHouses() => HouseDAO.GetAvailableHouses();

        public void UpdateHouseByHouseId(House house) => HouseDAO.UpdateHouseByHouseId(house);

        public void DeleteHouseByHouseId(int houseId) => HouseDAO.DeleteHouseByHouseId(houseId);
    }
}
