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
        public List<HouseDTO> GetAllHouses() => HouseDAO.GetAllHouses();
        public HouseDTO CreateHouse(CreateHouseDTO house)
        {
            AddressDTO address = AddressDAO.CreateAddress(house.Address, house.GoogleAddress);
            HouseDTO houseDTO = HouseDAO.CreateHouse(house.HouseName, house.Information, address.AddressId, (int)house.VillageId,
                house.LandlordId, (int)house.CampusId, house.PowerPrice, house.WaterPrice, (bool)house.FingerprintLock, 
                (bool)house.Camera, (bool)house.Parking, house.DistanceToCampus);
            return houseDTO;
        }
        //public List<HouseDTO> GetAllHouses() => HouseDAO.GetAllHouses();
        //public List<HouseDTO> GetHouseByName(string HouseName) => HouseDAO.GetHouseByName(HouseName);
        public HouseDTO GetHouseById(int HouseId) => HouseDAO.GetHouseById(HouseId);
        public void IncreaseView(int HouseId) => HouseDAO.IncreaseView(HouseId);
        public List<HouseDTO> GetListHousesByLandlordId(string LandlordId) => HouseDAO.GetListHousesByLandlordId(LandlordId);

        public decimal? GetMoneyForNotRentedRooms(int HouseId) => HouseDAO.GetMoneyForNotRentedRooms(HouseId);

        public int CountTotalHouse() => HouseDAO.CountTotalHouse();

        public int CountAvailableHouse() => HouseDAO.CountAvailableHouse();
        public List<AvailableHouseDTO> GetAvailableHouses() => HouseDAO.GetAvailableHouses();

        public void UpdateHouseByHouseId(House house) => HouseDAO.UpdateHouseByHouseId(house);

        public void DeleteHouseByHouseId(int houseId) => HouseDAO.DeleteHouseByHouseId(houseId);

        public List<ReportHouseDTO> GetListReportHouse() => HouseDAO.GetListReportHouse();

        public int CountTotalReportedHouse() => HouseDAO.CountTotalReportedHouse();
    }
}
