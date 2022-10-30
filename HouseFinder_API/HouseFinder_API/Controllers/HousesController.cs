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
using Microsoft.AspNetCore.OData.Query;

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

        //GET: api/Houses/availableHouses
        //[Home Page] List available Houses
        [HttpGet("availableHouses")]
        public ActionResult<IEnumerable<HouseDTO>> GetAvailableHouses() => housesRepository.GetAvailableHouses();

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

        //GET: api/Houses/GetHousesByLandlord?LandlordId=
        [HttpGet("GetHousesByLandlord")]
        public IActionResult GetListHousesByLandlordId(string LandlordId)
        {
            List<HouseDTO> houseDTOs = housesRepository.GetListHousesByLandlordId(LandlordId);
            if (houseDTOs == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(houseDTOs);
            }
        }

        //GET: api/Houses/GetMoneyForNotRentedRooms?HouseId=
        [HttpGet("GetMoneyForNotRentedRooms")]
        public Decimal? GetMoneyForNotRentedRooms(int HouseId)
        {
            Decimal? total = housesRepository.GetMoneyForNotRentedRooms(HouseId);
            return total;
        }

        //GET: api/Houses/CountTotalHouse
        [HttpGet("CountTotalHouse")]
        public int CountTotalHouse()
        {
            int totalHouse = housesRepository.CountTotalHouse();
            return totalHouse;
        }

        //GET: api/Houses/CountAvailableHouse
        //[Home Page] For Paging
        //[Staff - Dashboard] For statistic report
        [HttpGet("CountAvailableHouse")]
        public int CountAvailableHouse() => housesRepository.CountAvailableHouse();

    }
}
