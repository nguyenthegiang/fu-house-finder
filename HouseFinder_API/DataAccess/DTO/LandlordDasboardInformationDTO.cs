using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    //Detail information that got displayed in Landlord Dashboard & Staff List-Landlord
    public class LandlordDasboardInformationDTO
    {
        //Total Houses of 1 Landlord
        public int HouseCount { get; set; }
        //Total Rooms of 1 Landlord 
        public int RoomCount { get; set; }
        //Total Available Rooms of 1 Landlord
        public int RoomAvailableCount { get; set; }
    }
}
