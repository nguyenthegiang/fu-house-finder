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
    public class CommuneController : ControllerBase
    {
        private ICommuneRepository communeRepository = new CommuneRepository();

        [HttpGet("CountCommuneHavingHouse")]
        public int CountCommuneHavingHouse() => communeRepository.CountCommuneHavingHouse();

    }
}
