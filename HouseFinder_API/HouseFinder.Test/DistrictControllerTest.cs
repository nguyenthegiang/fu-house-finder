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
    public class DistrictControllerTest
    {
        #region GetAllDistricts

        /**
         * Method: GetAllDistricts()
         * Scenario: None
         * Expected behavior: Returns ActionResult
         */
        [Test]
        public void GetAllDistricts_Returns_ActionResult()
        {
            //ARRANGE
            var districtController = new DistrictController();

            //ACT
            var data = districtController.GetAllDistricts();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<DistrictDTO>>>(data);
        }

        /**
         * Method: GetAllDistricts()
         * Scenario: None
         * Expected behavior: Returns matching result data
         */
        [Test]
        public void GetAllDistricts_MatchResult()
        {
            //ARRANGE
            var districtController = new DistrictController();

            //ACT
            var data = districtController.GetAllDistricts();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<DistrictDTO>>>(data);

            List<DistrictDTO> results = data.Value.ToList();

            Assert.AreEqual("Huyện Thạch Thất", results[0].DistrictName);
            Assert.AreEqual("Huyện Quốc Oai", results[1].DistrictName);
        }

        #endregion GetAllDistricts

        #region CountDistrictHavingHouse

        /**
         * Method: CountDistrictHavingHouse()
         * Scenario: None
         * Expected behavior: Returns int
         */
        [Test]
        public void CountDistrictHavingHouse_Returns_Int()
        {
            //ARRANGE
            var districtController = new DistrictController();

            //ACT
            var data = districtController.CountDistrictHavingHouse();

            //ASSERT
            Assert.IsInstanceOf<int>(data);
        }

        /**
         * Method: CountDistrictHavingHouse()
         * Scenario: None
         * Expected behavior: Matching result data
         */
        [Test]
        public void CountDistrictHavingHouse_MatchResult()
        {
            //ARRANGE
            var districtController = new DistrictController();

            //ACT
            var data = districtController.CountDistrictHavingHouse();

            //ASSERT
            Assert.AreEqual(4, data);
        }

        #endregion CountDistrictHavingHouse
    }
}
