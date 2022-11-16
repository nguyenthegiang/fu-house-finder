using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IDistrictRepository
    {
        List<DistrictDTO> GetAllDistricts();
        public int CountDistrictHavingHouse();
    }
}
