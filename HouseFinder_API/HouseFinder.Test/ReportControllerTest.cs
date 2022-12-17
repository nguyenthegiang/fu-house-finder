using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccess.DTO;
using HouseFinder_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Transactions;
using FluentAssertions;

namespace HouseFinder.Test
{
    public class ReportControllerTest
    {
        #region GetAllReports

        /**
         * Method: GetAllReports()
         * Scenario: None
         * Expected behavior: Returns ActionResult
         */
        [Test]
        public void GetAllReports_Returns_ActionResult()
        {
            //ARRANGE
            var reportController = new ReportController();

            //ACT
            var data = reportController.GetAllReports();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<StaffReportDTO>>>(data);
        }

        /**
         * Method: GetAllReports()
         * Scenario: None
         * Expected behavior: Returns matching result data
         */
        [Test]
        public void GetAllReports_MatchResult()
        {
            //ARRANGE
            var reportController = new ReportController();

            //ACT
            var data = reportController.GetAllReports();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<StaffReportDTO>>>(data);

            List<StaffReportDTO> reports = data.Value.ToList();

            //Test matching data
            Assert.AreEqual("Chủ trọ tự ý vào phòng của bạn và tháo bóng đèn trong nhà vệ sinh của bạn", reports[1].ReportContent);
            Assert.AreEqual("Chủ trọ thu tiền điện vượt quá giá niêm yết", reports[4].ReportContent);
        }

        #endregion GetAllReports

        #region CountTotalReport

        /**
         * Method: CountTotalReport()
         * Scenario: None
         * Expected behavior: Returns int
         */
        [Test]
        public void CountTotalReport_Returns_Int()
        {
            //ARRANGE
            var ReportController = new ReportController();

            //ACT
            var data = ReportController.CountTotalReport();

            //ASSERT
            Assert.IsInstanceOf<int>(data);
        }

        /**
         * Method: CountTotalReport()
         * Scenario: None
         * Expected behavior: Matching result data
         */
        [Test]
        public void CountTotalReport_MatchResult()
        {
            //ARRANGE
            var ReportController = new ReportController();

            //ACT
            var data = ReportController.CountTotalReport();

            //ASSERT
            Assert.AreEqual(34, data);
        }

        #endregion CountTotalReport
    }
}
