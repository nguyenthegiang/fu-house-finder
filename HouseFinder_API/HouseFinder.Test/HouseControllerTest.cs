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

namespace HouseFinder.Test
{
    /**
     * [Test - HouseController]
     */
    [TestFixture]
    public class HouseControllerTest
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

        #region GetAvailableHouses

        /**
         * Method: GetAvailableHouses()
         * Scenario: None
         * Expected behavior: Returns ActionResult
         */
        [Test]
        public void GetAvailableHouses_Returns_ActionResult()
        {
            //ARRANGE
            var houseController = new HouseController();

            //ACT
            var data = houseController.GetAvailableHouses();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<AvailableHouseDTO>>>(data);
        }

        /**
         * Method: GetAvailableHouses()
         * Scenario: None
         * Expected behavior: Returns ActionResult
         */
        [Test]
        public void GetAvailableHouses_MatchResult()
        {
            //ARRANGE
            var houseController = new HouseController();

            //ACT
            var data = houseController.GetAvailableHouses();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<AvailableHouseDTO>>>(data);

            List<AvailableHouseDTO> results = data.Value.ToList();
        }

        #endregion GetAvailableHouses
    }
}
