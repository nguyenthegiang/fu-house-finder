using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DTO;
using HouseFinder_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using FluentAssertions;

namespace HouseFinder.Test
{
    [TestFixture]
    public class RoomControllerTest
    {
       
        private TransactionScope scope;
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
        #region GetAvailableRoomsByHouseId

        /**
         * Method: GetAllCampuses()
         * Scenario: None
         * Expected behavior: Returns ActionResult
         */
        [Test]
        public void GetAvailableRoomsByHouseId_Returns_ActionResult()
        {
            //ARRANGE
            var roomController = new RoomController();
            int roomId = 1;
            //ACT
            var data = roomController.GetAvailableRoomsByHouseId(roomId);

            //ASSERT
            Assert.IsInstanceOf<OkObjectResult>(data);
        }



          
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(1000)]
        public void GetListRoomByHouseId_InvalidId_Returns_NotFoundResult(int HouseId)
        {
            //ARRANGE
            var roomController = new RoomController();

            //ACT
            var data = roomController.GetAvailableRoomsByHouseId(HouseId);

            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            List<RoomDTO> rooms = okResult.Value.Should().BeAssignableTo<List<RoomDTO>>().Subject;

            //ASSERT
            Assert.AreEqual(0, rooms.Count);
        }

        /**
         * Method: GetAllCampuses()
         * Scenario: None
         * Expected behavior: Returns matching result data
         */
        [Test]
        public void GetAvailableRoomsByHouseId_ValidId_MatchResult()
        {
            //ARRANGE
            var roomController = new RoomController();
            int houseId = 1;
            //ACT

            var data = roomController.GetAvailableRoomsByHouseId(houseId);
            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            List<RoomDTO> results = okResult.Value.Should().BeAssignableTo<List<RoomDTO>>().Subject;
            //List<RoomDTO> results = data.Value.ToList();
            //ASSERT
            //Assert.IsInstanceOf<OkObjectResult>(data);

            //Assert.IsInstanceOf<NotFoundResult>(data);

            Assert.AreEqual("101", results[0].RoomName);
            Assert.AreEqual("202", results[1].RoomName);
            
        }

        #endregion GetAvailableRoomsByHouseId

    }
}
