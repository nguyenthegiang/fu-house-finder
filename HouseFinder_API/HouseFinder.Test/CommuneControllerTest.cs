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
    public class CommuneControllerTest
    {
        #region CountCommuneHavingHouse

        /**
         * Method: CountCommuneHavingHouse()
         * Scenario: None
         * Expected behavior: Returns int
         */
        [Test]
        public void CountCommuneHavingHouse_Returns_Int()
        {
            //ARRANGE
            var communeController = new CommuneController();

            //ACT
            var data = communeController.CountCommuneHavingHouse();

            //ASSERT
            Assert.IsInstanceOf<int>(data);
        }

        /**
         * Method: CountCommuneHavingHouse()
         * Scenario: None
         * Expected behavior: Matching result data
         */
        [Test]
        public void CountCommuneHavingHouse_MatchResult()
        {
            //ARRANGE
            var communeController = new CommuneController();

            //ACT
            var data = communeController.CountCommuneHavingHouse();

            //ASSERT
            Assert.AreEqual(4, data);
        }

        #endregion CountCommuneHavingHouse
    }
}
