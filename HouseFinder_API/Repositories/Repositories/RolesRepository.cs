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
    public class RolesRepository : IRolesRepository
    {
        public List<RoleDTO> GetStaffRoles() => RoleDAO.GetStaffRoles();
    }
}
