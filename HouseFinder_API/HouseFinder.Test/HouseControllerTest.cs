﻿using System;
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
using Repositories.IRepository;
using Repositories.Repositories;

namespace HouseFinder.Test
{
    /**
     * [Test - HouseController]
     */
    [TestFixture]
    public class HouseControllerTest
    {
        private TransactionScope scope;         //scope using for rollback

        //For Injection to Controller
        private IStorageRepository storageRepository;

        [SetUp]
        public void Setup()
        {
            scope = new TransactionScope();     //create scope

            /*
            //ARRANGE
            //Add some dummy data for test
            FUHouseFinderContext context = new FUHouseFinderContext();
            context.Houses.Add( new House()
                { HouseName = "Test House 1", View = 0, Information = "Test Info 1", AddressId = 1, VillageId = 1, LandlordId = "LA000001",
                  CampusId = 1, DistanceToCampus = 1, PowerPrice = 100, WaterPrice = 100, FingerprintLock = true, Camera = true, 
                  Parking = true, Deleted = false, CreatedDate = DateTime.UtcNow, LastModifiedDate = DateTime.UtcNow, CreatedBy = "LA000001",
                  LastModifiedBy = "LA000001",
                  Rooms = { new Room() { RoomName = "Test Room 1", PricePerMonth = 100, Information = "Test Info Room 1", AreaByMeters = 10,
                  Fridge = true, Kitchen = true, WashingMachine = true, Desk = true, NoLiveWithHost = true, Bed = true, ClosedToilet = true,
                  MaxAmountOfPeople = 1, CurrentAmountOfPeople = 1, BuildingNumber = 1, FloorNumber = 1, StatusId = 1, RoomTypeId = 1,
                  Deleted = false, CreatedDate = DateTime.UtcNow, LastModifiedDate = DateTime.UtcNow, CreatedBy = "LA000001", LastModifiedBy = "LA000001"
                  } } } );
            context.SaveChanges();
            */

            //Set up for Injection to Controller
            storageRepository = new StorageRepository("", "", "");
        }

        [TearDown]
        public void TearDown()
        {
            scope.Dispose();                    //dispose scope
        }

        #region GetAvailableHouses

        /**
         * Method: GetAvailableHouses()
         * Scenario: None
         * Expected behavior: Returns ActionResult
         */
        [Test]
        public void GetAvailableHouses_Returns_ActionResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.GetAvailableHouses();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<AvailableHouseDTO>>>(data);
        }

        /**
         * Method: GetAvailableHouses()
         * Scenario: None
         * Expected behavior: Returns matching result data
         */
        [Test]
        public void GetAvailableHouses_MatchResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.GetAvailableHouses();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<AvailableHouseDTO>>>(data);

            List<AvailableHouseDTO> houses = data.Value.ToList();

            //Test matching data
            Assert.AreEqual("Trọ Tâm Lê", houses[0].HouseName);
            Assert.AreEqual(50, houses[0].View);

            Assert.AreEqual("Trọ Tâm Thảo", houses[1].HouseName);
            Assert.AreEqual(34, houses[1].View);
        }

        #endregion GetAvailableHouses

        #region GetHouseById

        /**
         * Method: GetHouseById()
         * Scenario: Input HouseId: 1 (valid)
         * Expected behavior: Returns OkObjectResult
         */
        [Test]
        public void GetHouseById_ValidId_Returns_OkResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);
            int houseId = 1;

            //ACT
            var data = houseController.GetHouseById(houseId);

            //ASSERT
            Assert.IsInstanceOf<OkObjectResult>(data);
        }

        /**
         * Method: GetHouseById()
         * Scenario: Input HouseId: 0; -1; 1000 (invalid)
         * Expected behavior: Returns NotFoundResult
         */
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(1000)]
        public void GetHouseById_InvalidId_Returns_NotFoundResult(int HouseId)
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.GetHouseById(HouseId);

            //ASSERT
            Assert.IsInstanceOf<NotFoundResult>(data);
        }

        /**
         * Method: GetHouseById()
         * Scenario: Input HouseId: 2 (valid)
         * Expected behavior: Check matching result data
         */
        [Test]
        public void GetHouseById_ValidId_MatchResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);
            int houseId = 2;

            //ACT
            var data = houseController.GetHouseById(houseId);

            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            HouseDTO house = okResult.Value.Should().BeAssignableTo<HouseDTO>().Subject;

            //ASSERT
            Assert.AreEqual("Trọ Tâm Thảo", house.HouseName);
            Assert.AreEqual(34, house.View);
        }

        #endregion GetHouseById

        #region UpdateHouse

        /**
         * Method: UpdateHouse()
         * Scenario: Input House: valid
         * Expected behavior: Returns OkObjectResult
         */
        [Test]
        public void UpdateHouse_ValidData_OkObjectResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);
            int houseId = 2;

            //ACT

            //Get existing House
            var data = houseController.GetHouseById(houseId);
            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            HouseDTO existingHouse = okResult.Value.Should().BeAssignableTo<HouseDTO>().Subject;

            //Update House
            UpdateHouseDTO houseToUpdate = new UpdateHouseDTO();
            houseToUpdate.HouseId = existingHouse.HouseId;
            houseToUpdate.HouseName = "Trọ Tâm Thảo Test Update";   //update name
            houseToUpdate.Information = existingHouse.Information;
            houseToUpdate.Address = existingHouse.Address.Addresses;
            houseToUpdate.VillageId = (int)existingHouse.VillageId;
            houseToUpdate.CampusId = (int)existingHouse.CampusId;
            houseToUpdate.PowerPrice = existingHouse.PowerPrice;
            houseToUpdate.WaterPrice = existingHouse.WaterPrice;
            houseToUpdate.FingerprintLock = (bool)existingHouse.FingerprintLock;
            houseToUpdate.Camera = (bool)existingHouse.Camera;
            houseToUpdate.Parking = (bool)existingHouse.Parking;

            var updatedData = houseController.UpdateHouse(houseToUpdate);

            //ASSERT
            Assert.IsInstanceOf<OkResult>(updatedData);
        }

        /**
         * Method: UpdateHouse()
         * Scenario: Input House: invalid
         * Expected behavior: Returns BadRequestResult
         */
        [Test]
        public void UpdateHouse_InvalidData_BadRequestResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);
            int houseId = 2;

            //ACT

            //Get existing House
            var data = houseController.GetHouseById(houseId);
            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            HouseDTO existingHouse = okResult.Value.Should().BeAssignableTo<HouseDTO>().Subject;

            //Update House
            UpdateHouseDTO houseToUpdate = new UpdateHouseDTO();
            houseToUpdate.HouseId = existingHouse.HouseId;
            houseToUpdate.HouseName = null;   //update name invalid
            houseToUpdate.Information = existingHouse.Information;
            houseToUpdate.Address = existingHouse.Address.Addresses;
            houseToUpdate.VillageId = (int)existingHouse.VillageId;
            houseToUpdate.CampusId = (int)existingHouse.CampusId;
            houseToUpdate.PowerPrice = existingHouse.PowerPrice;
            houseToUpdate.WaterPrice = existingHouse.WaterPrice;
            houseToUpdate.FingerprintLock = (bool)existingHouse.FingerprintLock;
            houseToUpdate.Camera = (bool)existingHouse.Camera;
            houseToUpdate.Parking = (bool)existingHouse.Parking;

            var updatedData = houseController.UpdateHouse(houseToUpdate);

            //ASSERT
            Assert.IsInstanceOf<BadRequestObjectResult>(updatedData);
        }

        /**
         * Method: UpdateHouse()
         * Scenario: Input House: invalid - not found Id
         * Expected behavior: Returns BadRequestResult
         */
        [Test]
        public void UpdateHouse_NotFoundId_BadRequestResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);
            int houseId = 2;

            //ACT

            //Get existing House
            var data = houseController.GetHouseById(houseId);
            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            HouseDTO existingHouse = okResult.Value.Should().BeAssignableTo<HouseDTO>().Subject;

            //Update House
            UpdateHouseDTO houseToUpdate = new UpdateHouseDTO();
            houseToUpdate.HouseId = -1; //update id not exist
            houseToUpdate.HouseName = "Trọ Tâm Thảo Test Update";
            houseToUpdate.Information = existingHouse.Information;
            houseToUpdate.Address = existingHouse.Address.Addresses;
            houseToUpdate.VillageId = (int)existingHouse.VillageId;
            houseToUpdate.CampusId = (int)existingHouse.CampusId;
            houseToUpdate.PowerPrice = existingHouse.PowerPrice;
            houseToUpdate.WaterPrice = existingHouse.WaterPrice;
            houseToUpdate.FingerprintLock = (bool)existingHouse.FingerprintLock;
            houseToUpdate.Camera = (bool)existingHouse.Camera;
            houseToUpdate.Parking = (bool)existingHouse.Parking;

            var updatedData = houseController.UpdateHouse(houseToUpdate);

            //ASSERT
            Assert.IsInstanceOf<BadRequestObjectResult>(updatedData);
        }

        /**
         * Method: UpdateHouse()
         * Scenario: Input House: valid
         * Expected behavior: Check value got updated
         */
        [Test]
        public void UpdateHouse_ValidData_MatchUpdatedValue()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);
            int houseId = 2;

            //ACT

            //Get existing House
            var data = houseController.GetHouseById(houseId);
            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            HouseDTO existingHouse = okResult.Value.Should().BeAssignableTo<HouseDTO>().Subject;

            //Update House
            UpdateHouseDTO houseToUpdate = new UpdateHouseDTO();
            houseToUpdate.HouseId = existingHouse.HouseId;
            houseToUpdate.HouseName = "Trọ Tâm Thảo Test Update";   //update name
            houseToUpdate.Information = existingHouse.Information;
            houseToUpdate.Address = existingHouse.Address.Addresses;
            houseToUpdate.VillageId = (int)existingHouse.VillageId;
            houseToUpdate.CampusId = (int)existingHouse.CampusId;
            houseToUpdate.PowerPrice = existingHouse.PowerPrice;
            houseToUpdate.WaterPrice = existingHouse.WaterPrice;
            houseToUpdate.FingerprintLock = (bool)existingHouse.FingerprintLock;
            houseToUpdate.Camera = (bool)existingHouse.Camera;
            houseToUpdate.Parking = (bool)existingHouse.Parking;

            houseController.UpdateHouse(houseToUpdate);

            //Check updated data
            data = houseController.GetHouseById(houseId);
            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            HouseDTO updatedHouse = okResult.Value.Should().BeAssignableTo<HouseDTO>().Subject;
            //ASSERT
            Assert.AreEqual("Trọ Tâm Thảo Test Update", updatedHouse.HouseName);
        }

        #endregion UpdateHouse

        #region DeleteHouse

        /**
         * Method: DeleteHouse()
         * Scenario: Input HouseId: valid
         * Expected behavior: Returns OkResult
         */
        [Test]
        public void DeleteHouse_ValidData_OkResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);
            int houseId = 2;

            //ACT
            var data = houseController.DeleteHouse(houseId);

            //ASSERT
            Assert.IsInstanceOf<OkResult>(data);
        }

        /**
         * Method: DeleteHouse()
         * Scenario: Input HouseId: invalid
         * Expected behavior: Returns BadRequest
         */
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(2000)]
        public void DeleteHouse_InvalidData_BadRequestResult(int houseId)
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.DeleteHouse(houseId);

            //ASSERT
            Assert.IsInstanceOf<BadRequestResult>(data);
        }

        #endregion DeleteHouse

        #region IncreaseView

        /**
         * Method: IncreaseView()
         * Scenario: Input HouseId: 1 (valid)
         * Expected behavior: Returns OkResult
         */
        [Test]
        public void IncreaseView_ValidId_Returns_OkResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);
            int houseId = 1;

            //ACT
            var data = houseController.IncreaseView(houseId);

            //ASSERT
            Assert.IsInstanceOf<OkResult>(data);
        }

        /**
         * Method: IncreaseView()
         * Scenario: Input HouseId: -1, 0, 1000 (invalid)
         * Expected behavior: Not Found Result
         */
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1000)]
        public void IncreaseView_InvalidId_Returns_NotFoundResult(int houseId)
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.IncreaseView(houseId);

            //ASSERT
            Assert.IsInstanceOf<NotFoundResult>(data);
        }

        /**
         * Method: IncreaseView()
         * Scenario: Input HouseId: 2 (valid)
         * Expected behavior: Returns Check value got updated
         */
        [Test]
        public void IncreaseView_ValidId_MatchUpdatedValue()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);
            int houseId = 2;

            //Store old data
            var data = houseController.GetHouseById(houseId);
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            HouseDTO existingHouse = okResult.Value.Should().BeAssignableTo<HouseDTO>().Subject;
            int existingHouseView = (int)existingHouse.View;

            //ACT
            houseController.IncreaseView(houseId);

            //Check updated data
            data = houseController.GetHouseById(houseId);
            okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            HouseDTO updatedHouse = okResult.Value.Should().BeAssignableTo<HouseDTO>().Subject;
            int updatedHouseView = (int)updatedHouse.View;

            //ASSERT
            Assert.AreEqual(updatedHouseView, existingHouseView + 1);
        }

        #endregion IncreaseView

        #region GetListHousesByLandlordId

        /**
         * Method: GetListHousesByLandlordId()
         * Scenario: Input LandlordId: "LA000001" (valid)
         * Expected behavior: Returns OkObjectResult
         */
        [Test]
        public void GetListHousesByLandlordId_ValidId_Returns_OkObjectResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);
            string landlordId = "LA000001";

            //ACT
            var data = houseController.GetListHousesByLandlordId(landlordId);

            //ASSERT
            Assert.IsInstanceOf<OkObjectResult>(data);
        }

        /**
         * Method: GetListHousesByLandlordId()
         * Scenario: Input HouseId: Invalid
         * Expected behavior: Returns OkObjectResult with Empty List
         */
        [TestCase("")]
        [TestCase("LA000000")]
        [TestCase("LALALA")]
        [TestCase(".,--+!1@")]
        [TestCase(" ")]
        [TestCase("13456789")]
        [TestCase(null)]

        public void GetListHousesByLandlordId_InvalidId_Returns_EmptyList(string landlordId)
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.GetListHousesByLandlordId(landlordId);
            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            List<HouseDTO> houses = okResult.Value.Should().BeAssignableTo<List<HouseDTO>>().Subject;

            //ASSERT
            Assert.AreEqual(0, houses.Count);
        }

        /**
         * Method: GetListHousesByLandlordId()
         * Scenario: Input HouseId: Valid
         * Expected behavior: Check matching result data
         */
        [TestCase]

        public void GetListHousesByLandlordId_ValidId_MatchResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);
            string landlordId = "LA000001";

            //ACT
            var data = houseController.GetListHousesByLandlordId(landlordId);

            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            List<HouseDTO> houses = okResult.Value.Should().BeAssignableTo<List<HouseDTO>>().Subject;

            //ASSERT
            Assert.AreEqual("Trọ Tâm Lê", houses[0].HouseName);
            Assert.AreEqual(50, houses[0].View);

            Assert.AreEqual("Trọ Linh Lê", houses[1].HouseName);
            Assert.AreEqual(72, houses[1].View);
        }

        #endregion GetListHousesByLandlordId

        #region GetMoneyForNotRentedRooms

        /**
         * Method: GetMoneyForNotRentedRooms()
         * Scenario: Input HouseId: 1 (valid)
         * Expected behavior: Returns Decimal
         */
        [Test]
        public void GetMoneyForNotRentedRooms_ValidId_Returns_Decimal()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);
            int houseId = 1;

            //ACT
            var data = houseController.GetMoneyForNotRentedRooms(houseId);

            //ASSERT
            Assert.IsInstanceOf<Decimal>(data);
        }

        /**
         * Method: GetMoneyForNotRentedRooms()
         * Scenario: Input HouseId: invalid
         * Expected behavior: Returns 0
         */
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1000)]
        [TestCase(null)]
        public void GetMoneyForNotRentedRoomsInvalidId_Returns_Zero(int houseId)
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.GetMoneyForNotRentedRooms(houseId);

            //ASSERT
            Assert.IsInstanceOf<Decimal>(data);
        }

        /**
         * Method: GetMoneyForNotRentedRooms()
         * Scenario: Input HouseId: 1 (valid)
         * Expected behavior: Matching result data
         */
        [Test]
        public void GetMoneyForNotRentedRooms_ValidId_MatchResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);
            int houseId = 1;

            //ACT
            var data = houseController.GetMoneyForNotRentedRooms(houseId);

            //ASSERT
            Assert.AreEqual(16500000, data);
        }

        #endregion GetMoneyForNotRentedRooms

        #region CountTotalHouse

        /**
         * Method: CountTotalHouse()
         * Scenario: None
         * Expected behavior: Returns int
         */
        [Test]
        public void CountTotalHouse_Returns_Int()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.CountTotalHouse();

            //ASSERT
            Assert.IsInstanceOf<int>(data);
        }

        /**
         * Method: CountTotalHouse()
         * Scenario: None
         * Expected behavior: Matching result data
         */
        [Test]
        public void CountTotalHouse_MatchResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.CountTotalHouse();

            //ASSERT
            Assert.AreEqual(31, data);
        }

        #endregion CountTotalHouse

        #region CountAvailableHouse

        /**
         * Method: CountAvailableHouse()
         * Scenario: None
         * Expected behavior: Returns int
         */
        [Test]
        public void CountAvailableHouse_Returns_Int()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.CountAvailableHouse();

            //ASSERT
            Assert.IsInstanceOf<int>(data);
        }

        /**
         * Method: CountAvailableHouse()
         * Scenario: None
         * Expected behavior: Matching result data
         */
        [Test]
        public void CountAvailableHouse_MatchResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.CountAvailableHouse();

            //ASSERT
            Assert.AreEqual(29, data);
        }

        #endregion CountAvailableHouse

        #region CountTotalReportedHouse

        /**
         * Method: CountTotalReportedHouse()
         * Scenario: None
         * Expected behavior: Returns int
         */
        [Test]
        public void CountTotalReportedHouse_Returns_Int()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.CountTotalReportedHouse();

            //ASSERT
            Assert.IsInstanceOf<int>(data);
        }

        /**
         * Method: CountTotalReportedHouse()
         * Scenario: None
         * Expected behavior: Matching result data
         */
        [Test]
        public void CountTotalReportedHouse_MatchResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.CountTotalReportedHouse();

            //ASSERT
            Assert.AreEqual(11, data);
        }

        #endregion CountTotalReportedHouse

        #region GetReportedHouses

        /**
         * Method: GetReportedHouses()
         * Scenario: None
         * Expected behavior: Returns ActionResult
         */
        [Test]
        public void GetReportedHouses_Returns_ActionResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.GetReportedHouses();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<ReportHouseDTO>>>(data);
        }

        /**
         * Method: GetReportedHouses()
         * Scenario: None
         * Expected behavior: Returns matching result data
         */
        [Test]
        public void GetReportedHouses_MatchResult()
        {
            //ARRANGE
            var houseController = new HouseController(storageRepository);

            //ACT
            var data = houseController.GetReportedHouses();

            //ASSERT
            Assert.IsInstanceOf<ActionResult<IEnumerable<ReportHouseDTO>>>(data);

            List<ReportHouseDTO> houses = data.Value.ToList();

            //Test matching data
            Assert.AreEqual("Trọ Tâm Lê", houses[0].HouseName);
            Assert.AreEqual(5, houses[0].NumberOfReport);

            Assert.AreEqual("Trọ Tâm Thảo", houses[1].HouseName);
            Assert.AreEqual(1, houses[1].NumberOfReport);
        }

        #endregion GetReportedHouses
    }
}
