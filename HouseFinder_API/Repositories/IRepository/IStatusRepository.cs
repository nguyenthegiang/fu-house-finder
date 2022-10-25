using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IStatusRepository
    {
        public List<StatusDTO> GetStatusesByHouseId(int houseId);
    }
}
