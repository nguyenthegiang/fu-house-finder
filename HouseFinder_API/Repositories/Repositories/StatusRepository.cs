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
    public class StatusRepository : IStatusRepository
    {
        public List<StatusDTO> GetStatusesByHouseId(int houseId) => StatusDAO.GetStatusesByHouseId(houseId);
        public List<StatusDTO> GetAllStatus() => StatusDAO.GetAllListStatus();
    }
}
