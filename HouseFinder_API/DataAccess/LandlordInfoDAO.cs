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
        //Get Detail information that got displayed in Landlord Dashboard
        public static LandlordDasboardInformationDTO GetLandLordInfomationByLandlordId(string landlordId)
        {
            LandlordDasboardInformationDTO landLordInfo = new LandlordDasboardInformationDTO();
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get infomation of LandLord
                    List<House> housesOfLandlord = context.Houses.Where(h => h.LandlordId == landlordId).ToList();
                    //Count total houses
                    landLordInfo.HouseCount = housesOfLandlord.Count;

                    //count total rooms & available rooms
                    foreach(House house in housesOfLandlord)
                    {
                        landLordInfo.RoomCount += context.Rooms.Where(r => r.HouseId == house.HouseId).ToList().Count;
                        landLordInfo.RoomAvailableCount += context.Rooms.Where(r => r.HouseId == house.HouseId && r.StatusId == 1).ToList().Count;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return landLordInfo;
        }
    }
}
