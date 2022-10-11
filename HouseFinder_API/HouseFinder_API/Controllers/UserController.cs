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
    public class UserController : ControllerBase
    {
        private IUserReposiotry userReposiotry = new UserRepository();

        [HttpGet("{id}")]
        public IActionResult GetUserById(string id)
        {
            UserDTO userDTO = userReposiotry.GetUserByID(id);
            if (userDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(userDTO);
            }
        }
    }
}
