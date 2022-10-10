using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IHousesRepository
    {
        public List<House> GetAllHouses();
        public List<House> GetHouseByName(string name);
    }
}
