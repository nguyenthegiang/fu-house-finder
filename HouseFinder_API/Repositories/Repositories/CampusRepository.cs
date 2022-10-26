using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccess;
using DataAccess.DTO;

namespace Repositories
{
    public class CampusRepository : ICampusRepository
    {
        public List<CampusDTO> GetAllCampuses() => CampusDAO.GetAllCampuses();
    }
}
