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
    public class StatusController : ControllerBase
    {
        private IStatusRepository statusRepository = new StatusRepository();

        //GET: api/Status/getByHouseId?HouseId=
        [HttpGet("getByHouseId")]
        public IActionResult GetRStatusByHouseId(int HouseId)
        {
            List<StatusDTO> statusDTO = statusRepository.GetStatusesByHouseId(HouseId);
            if (statusDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(statusDTO);
            }
        }
    }
}
