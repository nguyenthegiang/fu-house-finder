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
    public class VillageControllerTest
    {
        #region CountVillageHavingHouse

        /**
         * Method: CountVillageHavingHouse()
         * Scenario: None
         * Expected behavior: Returns int
         */
        [Test]
        public void CountVillageHavingHouse_Returns_Int()
        {
            //ARRANGE
            var villageController = new VillageController();

            //ACT
            var data = villageController.CountVillageHavingHouse();

            //ASSERT
            Assert.IsInstanceOf<int>(data);
        }

        /**
         * Method: CountVillageHavingHouse()
         * Scenario: None
         * Expected behavior: Matching result data
         */
        [Test]
        public void CountVillageHavingHouse_MatchResult()
        {
            //ARRANGE
            var villageController = new VillageController();

            //ACT
            var data = villageController.CountVillageHavingHouse();

            //ASSERT
            Assert.AreEqual(5, data);
        }

        #endregion CountVillageHavingHouse
    }
}
