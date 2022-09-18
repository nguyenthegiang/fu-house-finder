using HouseFinder_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFinder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampusController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Campus>> GetAllCampuses()
        {
            List<Campus> campuses;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    campuses = context.Campuses.ToList();
                }
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return campuses;
        }
    }
}
