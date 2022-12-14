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
    public class DistrictController : ControllerBase
    {
        private IDistrictRepository districtRepository = new DistrictRepository();

        //GET: api/District
        //(Unused) Get List Districts
        //(with each District, all its Communes; and with each Commune, all its Villages)
        [HttpGet]
        public ActionResult<IEnumerable<DistrictDTO>> GetAllDistricts () => districtRepository.GetAllDistricts();

        [HttpGet("CountDistrictHavingHouse")]
        public int CountDistrictHavingHouse() => districtRepository.CountDistrictHavingHouse();

    }
}
