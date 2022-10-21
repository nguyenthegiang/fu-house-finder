using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFinder_API.Controllers
{
    [Route("roomType")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly FUHouseFinderContext _FUHouseFinderContext;

        public RoomTypeController(FUHouseFinderContext FUHouseFinderContext)
        {
            _FUHouseFinderContext = FUHouseFinderContext;
        }

        [EnableQuery]
        [HttpGet("Get")]
        public IActionResult Get()
        {
            return Ok(_FUHouseFinderContext.RoomTypes.AsQueryable());
        }
    }
}
