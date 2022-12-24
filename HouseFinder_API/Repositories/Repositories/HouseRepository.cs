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
            HouseDTO houseObj = HouseDAO.GetHouseById(houseDTO.HouseId);
            if (houseDTO == null)
            {
                throw new Exception();
            }
            House house = new House();
            house.HouseId = houseDTO.HouseId;
            house.HouseName = houseDTO.HouseName;
            house.Information = houseDTO.Information;
            house.VillageId = houseDTO.VillageId;
            house.CampusId = houseDTO.CampusId;
            house.PowerPrice = houseDTO.PowerPrice;
            house.WaterPrice = houseDTO.WaterPrice;
            house.FingerprintLock = houseDTO.FingerprintLock;
            house.Parking = houseDTO.Parking;
            house.Camera = houseDTO.Camera;
            house.LastModifiedDate = DateTime.Now;
            house.LastModifiedBy = houseDTO.ModifiedBy;

            house.AddressId = houseObj.AddressId;
            house.CreatedBy = houseObj.CreatedBy;
            house.LandlordId = houseObj.LandlordId;
            house.Deleted = houseObj.Deleted;
            house.DistanceToCampus = houseObj.DistanceToCampus;
            house.CreatedDate = houseObj.CreatedDate;

            HouseDAO.UpdateHouseByHouseId(house);

            AddressDTO addressObj = houseObj.Address;
            addressObj.Addresses = houseDTO.Address;

            Address address = new Address();
            address.Addresses = addressObj.Addresses;
            address.AddressId = addressObj.AddressId;
            address.Deleted = addressObj.Deleted;
            address.CreatedDate = (DateTime)addressObj.CreatedDate;
            address.GoogleMapLocation = addressObj.GoogleMapLocation;
            address.LastModifiedDate = DateTime.Now;

            AddressDAO.UpdateAddress(address);
        }

        public void DeleteHouseByHouseId(int houseId) => HouseDAO.DeleteHouseByHouseId(houseId);

        public List<ReportHouseDTO> GetListReportHouse() => HouseDAO.GetListReportHouse();

        public int CountTotalReportedHouse() => HouseDAO.CountTotalReportedHouse();
    }
}
