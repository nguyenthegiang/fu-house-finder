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
    public class RoomStatusRepository : IRoomStatusRepository
    {
        public List<RoomStatusDTO> GetStatusesByHouseId(int houseId) => RoomStatusDAO.GetStatusesByHouseId(houseId);
        public List<RoomStatusDTO> GetAllStatus() => RoomStatusDAO.GetAllListStatus();
    }
}
