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
    public class RateController : ControllerBase
    {
        private IRateRepository rateRepository = new RateRepository();

        /**
         * POST: api/Rate
         * [House Detail] Student gives rate & comments
         */
        [HttpPost]
        public IActionResult CreateRate(int houseId, int star, string comment)
        {
            try
            {
                //Create Rate from input info
                Rate rate = new Rate();
                rate.Star = star;
                rate.Comment = comment;
                rate.HouseId = houseId;

                //Get StudentId
                string userId = HttpContext.Session.GetString("User");
                if (userId == null)
                {
                    //user not logged in => throw error for alert
                    return Ok(new { Status = 403 });
                }

                //Check if this student has already rate (if yes -> not allow)
                List<RateDTO> rateDTOs = rateRepository.GetListRatesByHouseId(houseId);
                foreach (RateDTO rateDTO in rateDTOs)
                {
                    if (rateDTO.StudentId == userId)
                    {
                        return Ok(new { Status = 400 });
                    }
                }

                rate.StudentId = userId;

                //Default information
                rate.Deleted = false;
                rate.CreatedDate = DateTime.Now;
                rate.LastModifiedDate = DateTime.Now;
                rate.CreatedBy = userId;
                rate.LastModifiedBy = userId;

                //Add to Database
                rateRepository.CreateRate(rate);
                return Ok(new { Status = 200 });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /**
         * GET: api/Rate/GetListRatesByHouseId?HouseId=
         */
        [HttpGet("GetListRatesByHouseId")]
        public IActionResult GetListRatesByHouseId(int HouseId)
        {
            try
            {
                List<RateDTO> rateDTOs = rateRepository.GetListRatesByHouseId(HouseId);
                if (rateDTOs == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(rateDTOs);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //GET: api/Rate/GetRateById?RateId=
        [HttpGet("GetRateById")]
        public IActionResult GetRateById(int RateId)
        {
            RateDTO rateDTO = rateRepository.GetRateById(RateId);
            if (rateDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rateDTO);
            }
        }

        //PUT: api/Rate
        [HttpPut]
        public IActionResult ReplyComment(int rateId, string reply)
        {
            try
            {
                rateRepository.ReplyComment(rateId, reply);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
