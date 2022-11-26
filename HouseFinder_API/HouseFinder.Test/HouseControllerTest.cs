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

            //ARRANGE
            //Add some dummy data for test
            FUHouseFinderContext context = new FUHouseFinderContext();
            context.Houses.AddRange(
                new House()
                {
                    HouseName = "Test House 1",
                    View = 0,
                    Information = "Test Info 1",
                    AddressId = 1,
                    VillageId = 1,
                    LandlordId = "LA000001",
                    CampusId = 1,
                    DistanceToCampus = 1,
                    PowerPrice = 100,
                    WaterPrice = 100,
                    FingerprintLock = true,
                    Camera = true,
                    Parking = true,
                    Deleted = false,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    CreatedBy = "LA000001",
                    LastModifiedBy = "LA000001"
                },
                new House()
                {
                    HouseName = "Test House 2",
                    View = 0,
                    Information = "Test Info 2",
                    AddressId = 1,
                    VillageId = 1,
                    LandlordId = "LA000001",
                    CampusId = 1,
                    DistanceToCampus = 1,
                    PowerPrice = 100,
                    WaterPrice = 100,
                    FingerprintLock = true,
                    Camera = true,
                    Parking = true,
                    Deleted = false,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    CreatedBy = "LA000001",
                    LastModifiedBy = "LA000001"
                }
            );
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
