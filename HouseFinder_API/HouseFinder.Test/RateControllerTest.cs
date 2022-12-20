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
    public class RateControllerTest
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

        [Test]
        public void GetListRatesByHouseId_Returns_ActionResult()
        {
            //ARRANGE
            var rateController = new RateController();
            int houseId = 1;
            //ACT
            var data = rateController.GetListRatesByHouseId(houseId);

            //ASSERT
            Assert.IsInstanceOf<OkObjectResult>(data);
        }

        [Test]
        public void GetListRatesByHouseId_ValidId_MatchResult()
        {
            //ARRANGE
            var rateController = new RateController();
            int houseId = 1;
            //ACT

            var data = rateController.GetListRatesByHouseId(houseId);
            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            List<RateDTO> results = okResult.Value.Should().BeAssignableTo<List<RateDTO>>().Subject;
            //List<RoomDTO> results = data.Value.ToList();
            //ASSERT
            //Assert.IsInstanceOf<OkObjectResult>(data);

            //Assert.IsInstanceOf<NotFoundResult>(data);

            Assert.AreEqual("Rất tuyệt vời, gần trường nữa", results[0].Comment);

        }
    }
}
