using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DTO;
using HouseFinder_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Transactions;
using FluentAssertions;
using Repositories.IRepository;
using Repositories.Repositories;

namespace HouseFinder.Test
{
    [TestFixture]
    public class RoomControllerTest
    {
        private TransactionScope scope;

        //For Injection to Controller
        private IStorageRepository storageRepository;

        [SetUp]
        public void Setup()
        {
            scope = new TransactionScope();     //create scope

            //Set up for Injection to Controller
            storageRepository = new StorageRepository("", "", "");
        }

        [TearDown]
        public void TearDown()
        {
            scope.Dispose();                    //dispose scope
        }

        #region GetAvailableRoomsByHouseId
        [Test]
        public void GetAvailableRoomsByHouseId_Returns_ActionResult()
        {
            //ARRANGE
            var roomController = new RoomController(storageRepository);
            int houseId = 1;
            //ACT
            var data = roomController.GetAvailableRoomsByHouseId(houseId);

            //ASSERT
            Assert.IsInstanceOf<OkObjectResult>(data);
        }
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(1000)]
        public void GetListRoomByHouseId_InvalidId_Returns_NotFoundResult(int HouseId)
        {
            //ARRANGE
            var roomController = new RoomController(storageRepository);

            //ACT
            var data = roomController.GetAvailableRoomsByHouseId(HouseId);

            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            List<RoomDTO> rooms = okResult.Value.Should().BeAssignableTo<List<RoomDTO>>().Subject;

            //ASSERT
            Assert.AreEqual(0, rooms.Count);
        }

        [Test]
        public void GetAvailableRoomsByHouseId_ValidId_MatchResult()
        {
            //ARRANGE
            var roomController = new RoomController(storageRepository);
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

        #region GetRoomsByHouseId

        [Test]
        public void GetRoomsByHouseId_Returns_ActionResult()
        {
            //ARRANGE
            var roomController = new RoomController(storageRepository);
            int roomId = 1;
            //ACT
            var data = roomController.GetAvailableRoomsByHouseId(roomId);

            //ASSERT
            Assert.IsInstanceOf<OkObjectResult>(data);
        }
    
        [Test]
        public void RoomsByHouseId_ValidId_MatchResult()
        {
            //ARRANGE
            var roomController = new RoomController(storageRepository);
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

        }

        #endregion GetRoomsByHouseId
    }
}
