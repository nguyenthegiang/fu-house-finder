using BusinessObjects;
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
    public class ReportController : ControllerBase
    {
        private IReportRepository reportRepository = new ReportRepository();
        [HttpPost]
        public IActionResult Post([FromBody] Report report)
        {
            try
            {
                reportRepository.InsertReport(report);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
