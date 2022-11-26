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

            /*
            //ARRANGE
            //Add some dummy data for test
            FUHouseFinderContext context = new FUHouseFinderContext();
            context.Houses.Add(
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
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow,
                    CreatedBy = "LA000001",
                    LastModifiedBy = "LA000001",
                    Rooms =
                    {
                            new Room()
                            {
                                RoomName = "Test Room 1",
                                PricePerMonth = 100,
                                Information = "Test Info Room 1",
                                AreaByMeters = 10,
                                Fridge = true,
                                Kitchen = true,
                                WashingMachine = true,
                                Desk = true,
                                NoLiveWithHost = true,
                                Bed = true,
                                ClosedToilet = true,
                                MaxAmountOfPeople = 1,
                                CurrentAmountOfPeople = 1,
                                BuildingNumber = 1,
                                FloorNumber = 1,
                                StatusId = 1,
                                RoomTypeId = 1,
                                Deleted = false,
                                CreatedDate = DateTime.UtcNow,
                                LastModifiedDate = DateTime.UtcNow,
                                CreatedBy = "LA000001",
                                LastModifiedBy = "LA000001"
                            }
                    }
                }
            );
            context.SaveChanges();*/
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
         * Expected behavior: Returns matching result data
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

            Assert.AreEqual("Trọ Tâm Lê", results[0].HouseName);
            Assert.AreEqual(45, results[0].View);

            Assert.AreEqual("Trọ Tâm Thảo", results[1].HouseName);
            Assert.AreEqual(34, results[1].View);
        }

        #endregion GetAvailableHouses

        #region GetHouseById

        /**
         * Method: GetHouseById()
         * Scenario: Input HouseId: 1 (valid)
         * Expected behavior: Returns ActionResult
         */
        [Test]
        public void GetHouseById_ValidId_Returns_OkResult()
        {
            //ARRANGE
            var houseController = new HouseController();
            int houseId = 1;

            //ACT
            var data = houseController.GetHouseById(houseId);

            //ASSERT
            Assert.IsInstanceOf<OkObjectResult>(data);
        }

        #endregion GetHouseById
    }
}
