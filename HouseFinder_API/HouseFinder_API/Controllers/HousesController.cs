using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Repositories;
using DataAccess.DTO;

namespace HouseFinder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HousesController : ControllerBase
    {
        private IHousesRepository housesRepository = new HouseRepository();

        //GET: api/Houses
        [HttpGet]
        public ActionResult<IEnumerable<HouseDTO>> GetAllHouses() => housesRepository.GetAllHouses();

        //GET: api/Houses/search?name=
        [HttpGet("search")]
        public ActionResult<IEnumerable<HouseDTO>> GetHouseByName(string name)
        {
            return housesRepository.GetHouseByName(name);
        }

        //GET: api/Houses/HouseId
        [HttpGet("{HouseId}")]
        public IActionResult GetHouseById(int HouseId)
        {
            HouseDTO houseDTO = housesRepository.GetHouseById(HouseId);
            if (houseDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(houseDTO);
            }
        }
        //GET: api/Hoses/LandlordInfo?LandlordId=
        [HttpGet("LandlordInfo")]
        public IActionResult GetLandlordInfomationByLandlordId(string LandlordId)
        {
            LandlordDasboardInformationDTO l = new LandlordDasboardInformationDTO();
            l.HouseCount = housesRepository.GetHouseCountByLandlordId(LandlordId);
            l.RoomAvailableCount = 0;
            l.RoomCount = 0;
            if (l == null)
            {            
                return NotFound();
            }
            else
            {
                return Ok(l);
            }           
        }
    }
}
