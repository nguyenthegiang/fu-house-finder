using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;
using DataAccess.DTO;

namespace HouseFinder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampusController : ControllerBase
    {
        private ICampusRepository campusRepository = new CampusRepository();

        //GET: api/Campus
        //[Home Page] Get List Campuses to choose to filter
        [HttpGet]
        public ActionResult<IEnumerable<CampusDTO>> GetAllCampuses() => campusRepository.GetAllCampuses();
    }
}
