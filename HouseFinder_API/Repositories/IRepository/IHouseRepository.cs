using BusinessObjects;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IHouseRepository
    {
        public List<HouseDTO> GetAllHouses();
        public HouseDTO CreateHouse(CreateHouseDTO house);
        //public List<HouseDTO> GetAllHouses();
        //public List<HouseDTO> GetHouseByName(string HouseName);
        public HouseDTO GetHouseById(int HouseId);
        public void IncreaseView(int HouseId);
        public List<HouseDTO> GetListHousesByLandlordId(string LandlordId);
        public int CountTotalHouse();
        public decimal? GetMoneyForNotRentedRooms(int HouseId);
        public int CountAvailableHouse();
        public List<AvailableHouseDTO> GetAvailableHouses();

        public void UpdateHouseByHouseId(UpdateHouseDTO house);

        public void DeleteHouseByHouseId(int houseId);
        public List<ReportHouseDTO> GetListReportHouse();
        public int CountTotalReportedHouse();
    }
}
