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
        public HouseDTO CreateHouse(CreateHouseDTO houseDTO)
        {
            AddressDTO address = AddressDAO.CreateAddress(houseDTO.Address, houseDTO.GoogleAddress);
            House house = new House();
            house.HouseName = houseDTO.HouseName;
            house.Information = houseDTO.Information;
            house.AddressId = address.AddressId;
            house.VillageId = houseDTO.VillageId;
            house.LandlordId = houseDTO.LandlordId;
            house.CampusId = houseDTO.CampusId;
            house.PowerPrice = houseDTO.PowerPrice;
            house.WaterPrice = houseDTO.WaterPrice;
            house.FingerprintLock = houseDTO.FingerprintLock;
            house.Parking = houseDTO.Parking;
            house.Camera = houseDTO.Camera;
            house.DistanceToCampus = houseDTO.DistanceToCampus;
            house.View = 0;
            house.Deleted = false;
            house.CreatedDate = DateTime.UtcNow;
            house.LastModifiedDate = DateTime.UtcNow;
            house.CreatedBy = houseDTO.LandlordId;
            house.LastModifiedBy = houseDTO.LandlordId;
            return HouseDAO.CreateHouse(house);
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

        public void UpdateHouseByHouseId(UpdateHouseDTO houseDTO) {
            House house = new House();
            house.HouseName = houseDTO.HouseName;
            house.Information = houseDTO.Information;
            house.VillageId = houseDTO.VillageId;
            house.CampusId = houseDTO.CampusId;
            house.PowerPrice = houseDTO.PowerPrice;
            house.WaterPrice = houseDTO.WaterPrice;
            house.FingerprintLock = houseDTO.FingerprintLock;
            house.Parking = houseDTO.Parking;
            house.LastModifiedDate = DateTime.Now;
            house.LastModifiedBy = houseDTO.ModifiedBy;
            HouseDAO.UpdateHouseByHouseId(house); 
        }

        public void DeleteHouseByHouseId(int houseId) => HouseDAO.DeleteHouseByHouseId(houseId);

        public List<ReportHouseDTO> GetListReportHouse() => HouseDAO.GetListReportHouse();

        public int CountTotalReportedHouse() => HouseDAO.CountTotalReportedHouse();
    }
}
