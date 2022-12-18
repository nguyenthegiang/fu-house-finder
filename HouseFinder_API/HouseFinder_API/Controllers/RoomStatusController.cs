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
    public class RoomStatusController : ControllerBase
    {
        private IRoomStatusRepository statusRepository = new RoomStatusRepository();

        //GET: api/Status/getByHouseId?HouseId=
        [HttpGet("getByHouseId")]
        public IActionResult GetRoomStatusByHouseId(int HouseId)
        {
            List<RoomStatusDTO> statusDTO = statusRepository.GetStatusesByHouseId(HouseId);
            //if (statusDTO == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
                return Ok(statusDTO);
            //}
        }
        [HttpGet]
        public IActionResult GetAllStatus()
        {
            List <RoomStatusDTO> status = statusRepository.GetAllStatus();
            //if(status == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
                return Ok(status);
            //}
        }
    }
}
