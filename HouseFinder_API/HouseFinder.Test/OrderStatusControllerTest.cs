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
    public class OrderStatusControllerTest
    {
        #region GetAllOrderStatus

        /**
         * Method: GetAllOrderStatus()
         * Scenario: None
         * Expected behavior: Returns ActionResult
         */
        [Test]
        public void GetAllOrderStatus_Returns_ActionResult()
        {
            //ARRANGE
            var orderStatusController = new OrderStatusController();

            //ACT
            var data = orderStatusController.GetAllOrderStatus();

            //ASSERT
            Assert.IsInstanceOf<OkObjectResult>(data);
        }

        /**
         * Method: GetAllOrderStatus()
         * Scenario: None
         * Expected behavior: Check matching result data
         */
        [Test]
        public void GetAllOrderStatus_MatchResult()
        {
            //ARRANGE
            var orderStatusController = new OrderStatusController();

            //ACT
            var data = orderStatusController.GetAllOrderStatus();

            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            List<OrderStatusDTO> orders = okResult.Value.Should().BeAssignableTo<List<OrderStatusDTO>>().Subject;

            //ASSERT
            Assert.AreEqual("Unsolved", orders[0].StatusName);
            Assert.AreEqual("Processing", orders[1].StatusName);
            Assert.AreEqual("Solved", orders[2].StatusName);
        }

        #endregion GetAvailableHouses
    }
}
