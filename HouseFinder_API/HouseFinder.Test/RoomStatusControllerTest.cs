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
    public class RoomStatusControllerTest
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

        #region GetRoomStatusByHouseId

        /**
         * Method: GetRoomStatusByHouseId()
         * Scenario: Input HouseId: 1 (valid)
         * Expected behavior: Returns OkObjectResult
         */
        [Test]
        public void GetRoomStatusByHouseId_Returns_ActionResult()
        {
            //ARRANGE
            var roomStatusController = new RoomStatusController();
            int houseId = 1;

            //ACT
            var data = roomStatusController.GetRoomStatusByHouseId(houseId);

            //ASSERT
            Assert.IsInstanceOf<OkObjectResult>(data);
        }

        /**
         * Method: GetRoomStatusByHouseId()
         * Scenario: Input HouseId: 0; -1; 1000 (invalid)
         * Expected behavior: Returns NotFoundResult
         */
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(1000)]
        public void GetRoomStatusByHouseId_InvalidId_Returns_NotFoundResult(int HouseId)
        {
            //ARRANGE
            var roomStatusController = new RoomStatusController();

            //ACT
            var data = roomStatusController.GetRoomStatusByHouseId(HouseId);

            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            List<RoomStatusDTO> roomStatuses = okResult.Value.Should().BeAssignableTo<List<RoomStatusDTO>>().Subject;

            //ASSERT
            Assert.AreEqual(0, roomStatuses.Count);
        }

        /**
         * Method: GetRoomStatusByHouseId()
         * Scenario: Input HouseId: 2 (valid)
         * Expected behavior: Check matching result data
         */
        [Test]
        public void GetRoomStatusByHouseId_MatchResult()
        {
            //ARRANGE
            var roomStatusController = new RoomStatusController();
            int houseId = 2;

            //ACT
            var data = roomStatusController.GetRoomStatusByHouseId(houseId);

            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            List<RoomStatusDTO> roomStatus = okResult.Value.Should().BeAssignableTo<List<RoomStatusDTO>>().Subject;

            //ASSERT
            Assert.AreEqual("Available", roomStatus[0].StatusName);
            Assert.AreEqual("Disabled", roomStatus[1].StatusName);
        }

        #endregion GetRoomStatusByHouseId

        #region GetAllStatus

        /**
         * Method: GetAllStatus()
         * Scenario: None
         * Expected behavior: Returns OkObjectResult
         */
        [Test]
        public void GetAllStatus_Returns_ActionResult()
        {
            //ARRANGE
            var roomStatusController = new RoomStatusController();

            //ACT
            var data = roomStatusController.GetAllStatus();

            //ASSERT
            Assert.IsInstanceOf<OkObjectResult>(data);
        }

        /**
         * Method: GetAllStatus()
         * Scenario: None
         * Expected behavior: Check matching result data
         */
        [Test]
        public void GetAllStatus_MatchResult()
        {
            //ARRANGE
            var roomStatusController = new RoomStatusController();

            //ACT
            var data = roomStatusController.GetAllStatus();

            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            List<RoomStatusDTO> roomStatus = okResult.Value.Should().BeAssignableTo<List<RoomStatusDTO>>().Subject;

            //ASSERT
            Assert.AreEqual("Available", roomStatus[0].StatusName);
            Assert.AreEqual("Occupied", roomStatus[1].StatusName);
            Assert.AreEqual("Disabled", roomStatus[2].StatusName);
        }

        #endregion GetAllStatus
    }
}
