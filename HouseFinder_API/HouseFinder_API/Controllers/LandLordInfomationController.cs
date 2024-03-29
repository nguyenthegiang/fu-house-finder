﻿using DataAccess.DTO;
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

        /**
         * GET: api/Hoses/LandlordInfo?LandlordId=
         * [Staff]
         */
        [HttpGet("LandlordInfo")]
        public IActionResult GetLandlordInfomationByLandlordId(string LandlordId)
        {
            try
            {
                LandlordDasboardInformationDTO landlordInfo = landlordInfomationRepository.GetLandLordInfomationByLandlordId(LandlordId);
                return Ok(landlordInfo);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        /**
         * GET: api/Hoses/GetCurrentLandlordInfo
         * [Landlord/dahsboard]
         */
        [HttpGet("GetCurrentLandlordInfo")]
        public IActionResult GetCurrentLandlordInfo()
        {
            //Get UserId from Session
            string uid = HttpContext.Session.GetString("User");
            if (uid == null)
            {
                //user not logged in => throw error for alert
                return Ok(new { Status = 403 });
            }

            LandlordDasboardInformationDTO landlordInfo = landlordInfomationRepository.GetLandLordInfomationByLandlordId(uid);
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
