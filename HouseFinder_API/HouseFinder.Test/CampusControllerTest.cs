using BusinessObjects;
using DataAccess.DTO;
using HouseFinder_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;


namespace HouseFinder.Test
{
    /**
     * [Test - CampusController]
     */
    [TestFixture]
    public class CampusControllerTest
    {
        private TransactionScope scope;         //scope using for rollback

        [SetUp]
        public void Setup()
        {
            scope = new TransactionScope();     //create scope
        }

        [TearDown]
        public void TearDown()
        {
            scope.Dispose();                    //dispose scope
        }

        #region GetAllCampuses

        /**
         * Method: GetAllCampuses()
         * Scenario: None
         * Expected behavior: Returns ActionResult
         */
        [Test]
        public void GetAllCampuses_Returns_ActionResult()
        {
            //ARRANGE
            var campusController = new CampusController();

            //ACT
            var data = campusController.GetAllCampuses();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<CampusDTO>>>(data);
        }

        /**
         * Method: GetAllCampuses()
         * Scenario: None
         * Expected behavior: Returns matching result data
         */
        [Test]
        public void GetAllCampus_MatchResult()
        {
            //ARRANGE
            var campusController = new CampusController();

            //ACT
            var data = campusController.GetAllCampuses();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<CampusDTO>>>(data);

            List<CampusDTO> results = data.Value.ToList();

            Assert.AreEqual("FU - Hòa Lạc", results[0].CampusName);
            Assert.AreEqual("FU - Hồ Chí Minh", results[1].CampusName);
            Assert.AreEqual("FU - Đà Nẵng", results[2].CampusName);
            Assert.AreEqual("FU - Cần Thơ", results[3].CampusName);
            Assert.AreEqual("FU - Quy Nhơn", results[4].CampusName);
        }

        #endregion GetAllCampuses
    }
}