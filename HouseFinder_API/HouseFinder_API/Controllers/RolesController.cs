using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFinder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IRolesRepository rolesRepository = new RolesRepository();
        [HttpGet("staff")]
        public IActionResult GetStaffRoles()
        {
            List<RoleDTO> staffRoles = rolesRepository.GetStaffRoles();
            return Ok(staffRoles);
        }
    }
}
