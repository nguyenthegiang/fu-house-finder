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

            //ACT
            var data = roomController.GetAvailableRoomsByHouseId(1);

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<RoomDTO>>>(data);
        }

        /**
         * Method: GetAllCampuses()
         * Scenario: None
         * Expected behavior: Returns matching result data
         */
        [Test]
        public void GetAvailableRoomsByHouseId_MatchResult()
        {
            //ARRANGE
            var roomController = new RoomController();

            //ACT
            var data = roomController.GetAvailableRoomsByHouseId(1);

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<RoomDTO>>>(data);

            List<RoomDTO> results = data.Value.ToList();

            Assert.AreEqual("101", results[0].RoomName);
            Assert.AreEqual("102", results[1].RoomName);
            Assert.AreEqual("103", results[2].RoomName);
            Assert.AreEqual("201", results[3].RoomName);
            Assert.AreEqual("202", results[4].RoomName);
            Assert.AreEqual("203", results[5].RoomName);
            Assert.AreEqual("301", results[6].RoomName);
            Assert.AreEqual("302", results[7].RoomName);
            Assert.AreEqual("303", results[8].RoomName);
        }

        #endregion GetAvailableRoomsByHouseId
    }
}
