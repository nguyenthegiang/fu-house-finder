﻿using Microsoft.AspNetCore.Http;
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
using DataAccess;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace HouseFinder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private IHouseRepository houseRepository = new HouseRepository();
        private readonly IStorageRepository storageRepository;

        public HouseController(IStorageRepository storageRepository)
        {
            this.storageRepository = storageRepository;
        }

        /**
         * GET: api/Houses
         */
        [HttpGet]
        public ActionResult<IEnumerable<HouseDTO>> GetAllHouses() => houseRepository.GetAllHouses();

        /**
         * GET: api/Houses/availableHouses
         * [Home Page] List available Houses (using OData)
         */
        [EnableQuery]
        [HttpGet("availableHouses")]
        public ActionResult<IEnumerable<AvailableHouseDTO>> GetAvailableHouses()
        {
            try
            {
                List<AvailableHouseDTO> houseDTOs = houseRepository.GetAvailableHouses();

                //Get Image links from Server
                foreach (AvailableHouseDTO houseDTO in houseDTOs)
                {
                    foreach (var img in houseDTO.ImagesOfHouses)
                    {
                        img.ImageLink = storageRepository.RetrieveFile(img.ImageLink);
                    }
                }

                return houseDTOs;
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        //GET: api/Houses/search?name=
        //[HttpGet("search")]
        //public ActionResult<IEnumerable<HouseDTO>> GetHouseByName(string name)
        //{
        //    return housesRepository.GetHouseByName(name);
        //}

        /**
         *  GET: api/Houses/HouseId
         */
        [HttpGet("{HouseId}")]
        public IActionResult GetHouseById(int HouseId)
        {
            try
            {
                HouseDTO houseDTO = houseRepository.GetHouseById(HouseId);
                foreach (var img in houseDTO.ImagesOfHouses)
                {
                    img.ImageLink = storageRepository.RetrieveFile(img.ImageLink);
                }
                //if (houseDTO == null)
                //{
                //    return NotFound();
                //}
                //else
                //{
                return Ok(houseDTO);
                //}
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Policy = "Verified")]
        [HttpPost]
        public IActionResult CreateHouse(CreateHouseDTO house)
        {
            try
            {
                house.LandlordId = HttpContext.Session.GetString("User");
                if (String.IsNullOrWhiteSpace(house.LandlordId))
                {
                    return Forbid();
                }
                HouseDTO houseDTO = houseRepository.CreateHouse(house);
                return Ok(houseDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /**
         * PUT: api/Houses
         */
        [Authorize]
        [HttpPut]
        public IActionResult UpdateHouse(UpdateHouseDTO house)
        {
            var uid = HttpContext.Session.GetString("User");
            if (uid == null)
            {
                return Forbid();
            }
            HouseDTO houseDTO = houseRepository.GetHouseById(house.HouseId);
            if (houseDTO == null) return NotFound();
            if (!uid.Equals(houseDTO.LandlordId))
            {
                return Forbid();
            }
            house.ModifiedBy = uid;
            houseRepository.UpdateHouseByHouseId(house);
            return Ok();
        }

        /**
         * DELETE: api/House?houseId=
         */
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

        /**
         * POST: api/Houses/IncreaseView?HouseId=
         * [House Detail] 
         * Increase 'view' of this House by 1 when user click House Detail
         */
        [HttpPut("IncreaseView")]
        public IActionResult IncreaseView(int HouseId)
        {
            try
            {
                houseRepository.IncreaseView(HouseId);
                return Ok();
            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        /**
         * GET: api/Houses/GetHousesByLandlord?LandlordId=
         * [Staff/list-landlord]
         */
        [HttpGet("GetHousesByLandlord")]
        public IActionResult GetListHousesByLandlordId(string LandlordId)
        {
            try
            {
                List<HouseDTO> houseDTOs = houseRepository.GetListHousesByLandlordId(LandlordId);
                //if (houseDTOs == null)
                //{
                //    return NotFound();
                //}
                //else
                //{
                return Ok(houseDTOs);
                //}
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /**
         * GET: api/Houses/GetMoneyForNotRentedRooms?HouseId=
         */
        [HttpGet("GetMoneyForNotRentedRooms")]
        public Decimal? GetMoneyForNotRentedRooms(int HouseId) => houseRepository.GetMoneyForNotRentedRooms(HouseId);

        /**
         * GET: api/Houses/CountTotalHouse
         */
        [HttpGet("CountTotalHouse")]
        public int CountTotalHouse() => houseRepository.CountTotalHouse();

        /**
         * GET: api/Houses/CountAvailableHouse
         * [Staff - Dashboard] For statistic report
         */
        [HttpGet("CountAvailableHouse")]
        public int CountAvailableHouse() => houseRepository.CountAvailableHouse();

        /**
         * GET: api/House/CountTotalReportedHouse
         * [Home Page]
         * [Staff - Dashboard]
         */
        [HttpGet("CountTotalReportedHouse")]
        public int CountTotalReportedHouse() => houseRepository.CountTotalReportedHouse();

        /**
         * GET: api/House/GetReportedHouses
         */
        [EnableQuery]
        [HttpGet("GetReportedHouses")]
        public ActionResult<IEnumerable<ReportHouseDTO>> GetReportedHouses() => houseRepository.GetListReportHouse();
        //{
        //    List<ReportHouseDTO> houses = houseRepository.GetListReportHouse();
        //    if (houses == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return Ok(houses);
        //    }
        //}

        /**
         * GET: api/Houses/Distance
         * Demo Calculate Distance by calling Google Map API
         */
        [HttpGet("Distance")]
        public async Task<IActionResult> CalculateDistanceOfHouse(int HouseId)
        {
            try
            {
                HouseDTO houseDTO = houseRepository.GetHouseById(HouseId);
                if (houseDTO == null)
                {
                    return NotFound();
                }

                //Get Coordinate of 2 Locations
                string houseLocation = houseDTO.Address.GoogleMapLocation;
                string campusLocation = CampusDAO.GetCampusById((int)houseDTO.CampusId).Address.GoogleMapLocation;

                /**
                 * Use HttpClient to call Google Map API to get Distance
                 */

                //Make URL
                string googleMapApiUrl = "https://maps.googleapis.com/maps/api/distancematrix/json" +
                    $"?destinations={campusLocation}" +
                    $"&origins={houseLocation}" +
                    "&key=AIzaSyCcM7drUM9PLPlnALFvsckkvythVzLhpgA";

                // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
                HttpClient client = new HttpClient();

                // Call asynchronous network methods in a try/catch block to handle exceptions.
                try
                {
                    using HttpResponseMessage response = await client
                        .GetAsync(googleMapApiUrl);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return Ok(responseBody);
                }
                catch (HttpRequestException e)
                {
                    return BadRequest(e.Message);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /**
         * GET: api/Houses/GetListHouseOfCurrentLandlord
         * [Landlord/dashboard]
         */
        [HttpGet("GetListHouseOfCurrentLandlord")]
        public IActionResult GetListHouseOfCurrentLandlord()
        {
            try
            {
                //Get UserId from Session
                string uid = HttpContext.Session.GetString("User");
                if (uid == null)
                {
                    //user not logged in => throw error for alert
                    return Ok(new { Status = 403 });
                }

                //Get list house
                List<HouseDTO> houseDTOs = houseRepository.GetListHousesByLandlordId(uid);
                if (houseDTOs == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(houseDTOs);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
