using BusinessObjects;
using HouseFinder_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;


namespace HouseFinder.Test
{
    /**
     * [Test - CampusController]
     */
    [TestFixture]
    public class CampusControllerTest
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

        #region GetAllCampuses

        [Test]
        public void GetAllCampuses_Returns_OkResult()
        {
            //ARRANGE
            var campusController = new CampusController();

            //ACT
            var data = campusController.GetAllCampuses();

            //ASSERT
            Assert.IsInstanceOf<OkResult>(data);
        }

        #endregion GetAllCampuses
    }
}