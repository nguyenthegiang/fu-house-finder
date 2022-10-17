using AutoMapper;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class LandlordInfoDAO
    {
        public static LandlordDasboardInformationDTO GetLandLordInfomationByLandlordId(string landlordId)
        {
            LandlordDasboardInformationDTO landLord = new LandlordDasboardInformationDTO();
            int houseCount=0;
            int roomCount=0;
            int roomAvailableCount=0;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //include Address into Houses
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    //Get infomation of LandLord
                    
                    houseCount = context.Houses.Where(p => p.LandlordId == landlordId).ToList().Count;
                    List<House> houses = context.Houses.Where(h => h.LandlordId == landlordId).ToList();
                    foreach(House h in houses)
                    {
                        int r = context.Rooms.Where(r => r.HouseId == h.HouseId).ToList().Count;
                        roomCount = roomCount + r;
                        int ra = context.Rooms.Where(r => r.HouseId == h.HouseId && r.StatusId == 1).ToList().Count;
                        roomAvailableCount = roomAvailableCount + ra;
                    }
                    landLord.HouseCount = houseCount;
                    landLord.RoomCount = roomCount;
                    landLord.RoomAvailableCount = roomAvailableCount;
                    

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return landLord;
        }
    }
}
