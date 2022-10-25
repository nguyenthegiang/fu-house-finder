using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class StatusDAO
    {

        //[Landlord][List room] Get list Status for Filter
        public static List<StatusDTO> GetStatusesByHouseId(int houseId)
        {
            List<StatusDTO> statusesOfHouse = new List<StatusDTO>();
            List<RoomDTO> rooms = RoomDAO.GetRoomsByHouseId(houseId);
            try
            {
                foreach (RoomDTO room in rooms)
                {
                    statusesOfHouse.Add(room.Status);
                }

                statusesOfHouse = statusesOfHouse.GroupBy(status => status.StatusId).Select(status => status.First()).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return statusesOfHouse;
        }
    }
}
