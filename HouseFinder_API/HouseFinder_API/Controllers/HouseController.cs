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
    public class HouseController : ControllerBase
    {
        private IHouseRepository houseRepository = new HouseRepository();

        //GET: api/Houses
        //[HttpGet]
        //public ActionResult<IEnumerable<HouseDTO>> GetAllHouses() => housesRepository.GetAllHouses();

        //GET: api/Houses/availableHouses
        //[Home Page] List available Houses (using OData)
        [EnableQuery]
        [HttpGet("availableHouses")]
        public ActionResult<IEnumerable<AvailableHouseDTO>> GetAvailableHouses() => houseRepository.GetAvailableHouses();

        //GET: api/Houses/search?name=
        //[HttpGet("search")]
        //public ActionResult<IEnumerable<HouseDTO>> GetHouseByName(string name)
        //{
        //    return housesRepository.GetHouseByName(name);
        //}

        //GET: api/Houses/HouseId
        [HttpGet("{HouseId}")]
        public IActionResult GetHouseById(int HouseId)
        {
            HouseDTO houseDTO = houseRepository.GetHouseById(HouseId);
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
            List<HouseDTO> houseDTOs = houseRepository.GetListHousesByLandlordId(LandlordId);
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
            Decimal? total = houseRepository.GetMoneyForNotRentedRooms(HouseId);
            return total;
        }

        //GET: api/Houses/CountTotalHouse
        [HttpGet("CountTotalHouse")]
        public int CountTotalHouse()
        {
            int totalHouse = houseRepository.CountTotalHouse();
            return totalHouse;
        }

        //GET: api/Houses/CountAvailableHouse
        //[Home Page] For Paging
        //[Staff - Dashboard] For statistic report
        [HttpGet("CountAvailableHouse")]
        public int CountAvailableHouse() => houseRepository.CountAvailableHouse();


        //PUT: api/Houses
        [HttpPut]
        public IActionResult UpdateHouseByHouseId(House house)
        {
            try
            {
                houseRepository.UpdateHouseByHouseId(house);
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        //DELETE: api/House?houseId=
        [HttpDelete()]
        public IActionResult DeleteHouse(int houseId)
        {
            try
            {
                houseRepository.DeleteHouseByHouseId(houseId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //GET: api/Houses/Distance
        [HttpGet("Distance")]
        public IActionResult CalculateDistanceOfHouse(int HouseId)
        {
            try
            {
                HouseDTO houseDTO = houseRepository.GetHouseById(HouseId);
                if (houseDTO == null)
                {
                    return NotFound();
                }

                return Ok();
            } catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
