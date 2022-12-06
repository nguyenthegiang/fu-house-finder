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
        private IUserRepository userReposiotry = new UserRepository();

        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<StaffReportDTO>> GetAllReports() => reportRepository.GetAllReports();

        //[Report] POST: Add Report
        [HttpPost]
        public IActionResult Post( Report report)
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

                //Set default date
                report.ReportedDate = DateTime.Now;
                report.SolvedDate = null;
                report.StudentId = uid;
                //Add to DB
                reportRepository.AddReport(report);

                return Ok(new { Status = 200 });
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


        //GET: api/Report/SearchReportByHouseName/
        [HttpGet("SearchReportByHouseName/{key}")]
        public ActionResult<IEnumerable<StaffReportDTO>> SearchReportByHouseName(string key)
        {
            List<StaffReportDTO> reports = reportRepository.SearchReportByName(key);
            if (reports == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(reports);
            }
        }

        //GET: api/Report/CountTotalReportByHouseId/
        [HttpGet("CountTotalReportByHouseId/{houseId}")]
        public int CountTotalReportByHouseId(int houseId)
        {
            return reportRepository.CountReportByHouseId(houseId);
        }

        //GET: api/Report/CountTotalReport
        [HttpGet("CountTotalReport")]
        public int CountTotalReport()
        {
            return reportRepository.CounTotalReport();
        }

    }
}
