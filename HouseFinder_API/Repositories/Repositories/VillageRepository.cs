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
    public class VillageRepository : IVillageRepository
    {
        public int CountVillageHavingHouse() => VillageDAO.CountVillageHavingHouse();

        public VillageDTO GetVillageByName(string name) => VillageDAO.GetVillageByName(name);
    }
}
