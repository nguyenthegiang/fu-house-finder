using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace HouseFinder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampusController : ControllerBase
    {
        private ICampusRepository campusRepository = new CampusRepository();

        //GET: api/Campus
        [HttpGet]
        public ActionResult<IEnumerable<Campus>> GetAllCampuses() => campusRepository.GetAllCampuses();
    }
}
