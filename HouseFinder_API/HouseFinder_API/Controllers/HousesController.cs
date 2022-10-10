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

        [HttpGet("search")]
        public ActionResult<IEnumerable<House>> GetHouseByName(string name)
        {
            return housesRepository.GetHouseByName(name);
        }

    }
}
