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
    public class LandLordInfomationController : ControllerBase
    {
        private ILandlordInfomationRepository landlordInfomationRepository = new LandlordInfomationRepository();

        //GET: api/Hoses/LandlordInfo?LandlordId=
        [HttpGet("LandlordInfo")]
        public IActionResult GetLandlordInfomationByLandlordId(string LandlordId)
        {
            LandlordDasboardInformationDTO landlordInfo = landlordInfomationRepository.GetLandLordInfomationByLandlordId(LandlordId);
            if (landlordInfo == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(landlordInfo);
            }
        }
    }
}
