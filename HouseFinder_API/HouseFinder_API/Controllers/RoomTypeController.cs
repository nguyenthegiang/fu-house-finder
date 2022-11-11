using BusinessObjects;
using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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
    public class RoomTypeController : ControllerBase
    {
        private IRoomTypeRepository roomTypeRepository = new RoomTypeRepository();

        /**
         * GET: api/RoomType
         Return list of all RoomTypes in the Database
         */
        [HttpGet]
        public IActionResult GetRoomTypes()
        {
            List<RoomTypeDTO> roomTypesDTO = roomTypeRepository.GetRoomTypes();
            if (roomTypesDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(roomTypesDTO);
            }
        }

        /**
         * GET: api/RoomType/getByHouseId?HouseId=
         Get all roomTypes that this house has
         */
        [HttpGet("getByHouseId")]
        public IActionResult GetRoomTypesByHouseId(int HouseId)
        {
            List<RoomTypeDTO> roomTypesDTO = roomTypeRepository.GetRoomTypesByHouseId(HouseId);
            if (roomTypesDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(roomTypesDTO);
            }
        }
    }
}
