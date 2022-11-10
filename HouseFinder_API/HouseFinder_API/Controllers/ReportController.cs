using BusinessObjects;
using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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

        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<StaffReportDTO>> GetAllOrders() => reportRepository.GetAllReport();

        //[Report] POST: Add Report
        [HttpPost]
        public IActionResult Post([FromBody] Report report)
        {
            try
            {
                //Set default date
                report.CreatedDate = DateTime.Now;
                report.LastModifiedDate = DateTime.Now;

                //Add to DB
                reportRepository.AddReport(report);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //GET: api/Report/GetTotalReportByMonth
        [HttpGet("GetTotalReportByMonth")]
        public int[] GetTotalReportByMonth()
        {
            int[] totals = reportRepository.GetTotalReportByMonth();
            return totals;
        }



    }
}
