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
    public class OrderControllerTest
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

        #region GetAllOrders

        /**
         * Method: GetAllOrders()
         * Scenario: None
         * Expected behavior: Returns ActionResult
         */
        [Test]
        public void GetAllOrders_Returns_ActionResult()
        {
            //ARRANGE
            var orderController = new OrderController();

            //ACT
            var data = orderController.GetAllOrders();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<OrderDTO>>>(data);
        }

        /**
         * Method: GetAllOrders()
         * Scenario: None
         * Expected behavior: Returns matching result data
         */
        [Test]
        public void GetAllOrders_MatchResult()
        {
            //ARRANGE
            var orderController = new OrderController();

            //ACT
            var data = orderController.GetAllOrders();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<OrderDTO>>>(data);

            List<OrderDTO> orders = data.Value.ToList();

            //Test matching data
            Assert.AreEqual("Bùi Ngọc Huyền", orders[0].StudentName);
            Assert.AreEqual("Trần Thị Nguyệt Hà", orders[3].StudentName);
        }

        #endregion GetAllOrders

        #region CountTotalOrder

        /**
         * Method: CountTotalOrder()
         * Scenario: None
         * Expected behavior: Returns int
         */
        [Test]
        public void CountTotalOrder_Returns_Int()
        {
            //ARRANGE
            var orderController = new OrderController();

            //ACT
            var data = orderController.CountTotalOrder();

            //ASSERT
            Assert.IsInstanceOf<int>(data);
        }

        /**
         * Method: CountTotalOrder()
         * Scenario: None
         * Expected behavior: Matching result data
         */
        [Test]
        public void CountTotalOrder_MatchResult()
        {
            //ARRANGE
            var orderController = new OrderController();

            //ACT
            var data = orderController.CountTotalOrder();

            //ASSERT
            Assert.AreEqual(37, data);
        }

        #endregion CountTotalOrder
    }
}
