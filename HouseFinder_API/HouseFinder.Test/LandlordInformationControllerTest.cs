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
    public class LandlordInformationControllerTest
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
        public void GetLandlordInfomationByLandlordId_Returns_ActionResult()
        {
            //ARRANGE
            var landlordInfomationController = new LandLordInfomationController();
            string landLordId = "LA000001";
            //ACT
            var data = landlordInfomationController.GetLandlordInfomationByLandlordId(landLordId);

            //ASSERT
            Assert.IsInstanceOf<OkObjectResult>(data);
        }

        [Test]
        public void GetLandlordInfomationByLandlordId_ValidId_MatchResult()
        {
            //ARRANGE
            var landlordInfomationController = new LandLordInfomationController();
            string landLordId = "LA000001";
            //ACT

            var data = landlordInfomationController.GetLandlordInfomationByLandlordId(landLordId);
            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            LandlordDasboardInformationDTO results = okResult.Value.Should().BeAssignableTo<LandlordDasboardInformationDTO>().Subject;
            //List<RoomDTO> results = data.Value.ToList();
            //ASSERT
            //Assert.IsInstanceOf<OkObjectResult>(data);

            //Assert.IsInstanceOf<NotFoundResult>(data);

            Assert.AreEqual(4, results.HouseCount);
           

        }
    }
}
