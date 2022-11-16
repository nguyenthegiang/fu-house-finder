using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IVillageRepository
    {
        VillageDTO GetVillageByName(string name);
        public int CountVillageHavingHouse();

    }
}
