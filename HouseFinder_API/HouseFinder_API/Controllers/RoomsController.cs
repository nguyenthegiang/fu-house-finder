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
    public class RoomsController : ControllerBase
    {
        private IRoomsRepository roomsRepository = new RoomRepository();

        //GET: api/Rooms/getByHouseId?HouseId=
        [HttpGet("getByHouseId")]
        public IActionResult GetRoomsByHouseId(int HouseId)
        {
            List<RoomDTO> roomsDTO = roomsRepository.GetRoomsByHouseId(HouseId);
            if (roomsDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(roomsDTO);
            }
        }
    }
}
