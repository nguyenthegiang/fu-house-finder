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
    public class RoomTypeControllerTest
    {
        #region GetRoomTypesByHouseId

        /**
         * Method: GetRoomTypesByHouseId()
         * Scenario: Input HouseId: 1 (valid)
         * Expected behavior: Returns OkObjectResult
         */
        [Test]
        public void GetRoomTypesByHouseId_Returns_ActionResult()
        {
            //ARRANGE
            var roomTypeController = new RoomTypeController();
            int houseId = 1;

            //ACT
            var data = roomTypeController.GetRoomTypesByHouseId(houseId);

            //ASSERT
            Assert.IsInstanceOf<OkObjectResult>(data);
        }

        /**
         * Method: GetRoomTypesByHouseId()
         * Scenario: Input HouseId: 0; -1; 1000 (invalid)
         * Expected behavior: Returns NotFoundResult
         */
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(1000)]
        public void GetRoomTypesByHouseId_InvalidId_Returns_NotFoundResult(int HouseId)
        {
            //ARRANGE
            var roomTypeController = new RoomTypeController();

            //ACT
            var data = roomTypeController.GetRoomTypesByHouseId(HouseId);

            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            List<RoomTypeDTO> roomTypes = okResult.Value.Should().BeAssignableTo<List<RoomTypeDTO>>().Subject;

            //ASSERT
            Assert.AreEqual(0, roomTypes.Count);
        }

        /**
         * Method: GetRoomStatusByHouseId()
         * Scenario: Input HouseId: 2 (valid)
         * Expected behavior: Check matching result data
         */
        [Test]
        public void GetRoomTypesByHouseId_MatchResult()
        {
            //ARRANGE
            var roomTypeController = new RoomTypeController();
            int houseId = 2;

            //ACT
            var data = roomTypeController.GetRoomTypesByHouseId(houseId);

            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            List<RoomTypeDTO> roomTypes = okResult.Value.Should().BeAssignableTo<List<RoomTypeDTO>>().Subject;

            //ASSERT
            Assert.AreEqual("Không khép kín", roomTypes[0].RoomTypeName);
            Assert.AreEqual("Chung cư mini", roomTypes[1].RoomTypeName);
        }

        #endregion GetAvailableHouses

        #region GetRoomTypes

        /**
         * Method: GetRoomTypes()
         * Scenario: None
         * Expected behavior: Returns OkObjectResult
         */
        [Test]
        public void GetRoomTypes_Returns_ActionResult()
        {
            //ARRANGE
            var roomTypeController = new RoomTypeController();

            //ACT
            var data = roomTypeController.GetRoomTypes();

            //ASSERT
            Assert.IsInstanceOf<OkObjectResult>(data);
        }

        /**
         * Method: GetRoomTypes()
         * Scenario: None
         * Expected behavior: Check matching result data
         */
        [Test]
        public void GetRoomTypes_MatchResult()
        {
            //ARRANGE
            var roomTypeController = new RoomTypeController();

            //ACT
            var data = roomTypeController.GetRoomTypes();

            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            List<RoomTypeDTO> roomTypes = okResult.Value.Should().BeAssignableTo<List<RoomTypeDTO>>().Subject;

            //ASSERT
            Assert.AreEqual("Khép kín", roomTypes[0].RoomTypeName);
            Assert.AreEqual("Không khép kín", roomTypes[1].RoomTypeName);
            Assert.AreEqual("Chung cư mini", roomTypes[2].RoomTypeName);
        }

        #endregion GetAvailableHouses
    }
}
