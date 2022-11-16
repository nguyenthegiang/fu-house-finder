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
    public class VillageController : ControllerBase
    {
        private IVillageRepository villageRepository = new VillageRepository();

        [HttpGet("CountVillageHavingHouse")]
        public int CountVillageHavingHouse() => villageRepository.CountVillageHavingHouse();

    }
}
