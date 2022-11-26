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
    /**
     * [Test - HouseController]
     */
    [TestFixture]
    public class HouseControllerTest
    {
        private TransactionScope scope;         //scope using for rollback

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
            var houseController = new HouseController();

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
            var houseController = new HouseController();

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
            var houseController = new HouseController();
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
            var houseController = new HouseController();

            //ACT
            var data = houseController.GetHouseById(HouseId);

            //ASSERT
            Assert.IsInstanceOf<NotFoundResult>(data);
        }

        /**
         * Method: GetHouseById()
         * Scenario: Input HouseId: 2 (valid)
         * Expected behavior: Returns OkObjectResult
         */
        [Test]
        public void GetHouseById_ValidId_MatchResult()
        {
            //ARRANGE
            var houseController = new HouseController();
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
            var houseController = new HouseController();
            int houseId = 2;

            //ACT

            //Get existing House
            var data = houseController.GetHouseById(houseId);
            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            HouseDTO existingHouse = okResult.Value.Should().BeAssignableTo<HouseDTO>().Subject;

            //Update House
            House houseToUpdate = new House();
            houseToUpdate.HouseId = existingHouse.HouseId;
            houseToUpdate.HouseName = "Trọ Tâm Thảo Test Update";   //update name
            houseToUpdate.View = existingHouse.View;
            houseToUpdate.Information = existingHouse.Information;
            houseToUpdate.AddressId = existingHouse.AddressId;
            houseToUpdate.VillageId = existingHouse.VillageId;
            houseToUpdate.LandlordId = existingHouse.LandlordId;
            houseToUpdate.CampusId = existingHouse.CampusId;
            houseToUpdate.DistanceToCampus = existingHouse.DistanceToCampus;
            houseToUpdate.PowerPrice = existingHouse.PowerPrice;
            houseToUpdate.WaterPrice = existingHouse.WaterPrice;
            houseToUpdate.FingerprintLock = existingHouse.FingerprintLock;
            houseToUpdate.Camera = existingHouse.Camera;
            houseToUpdate.Parking = existingHouse.Parking;
            houseToUpdate.Deleted = existingHouse.Deleted;
            houseToUpdate.CreatedDate = existingHouse.CreatedDate;
            houseToUpdate.LastModifiedDate = existingHouse.LastModifiedDate;
            houseToUpdate.CreatedBy = existingHouse.CreatedBy;
            houseToUpdate.LastModifiedBy = existingHouse.LastModifiedBy;

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
            var houseController = new HouseController();
            int houseId = 2;

            //ACT

            //Get existing House
            var data = houseController.GetHouseById(houseId);
            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            HouseDTO existingHouse = okResult.Value.Should().BeAssignableTo<HouseDTO>().Subject;

            //Update House
            House houseToUpdate = new House();
            houseToUpdate.HouseId = existingHouse.HouseId;
            houseToUpdate.HouseName = null;   //update name
            houseToUpdate.View = existingHouse.View;
            houseToUpdate.Information = existingHouse.Information;
            houseToUpdate.AddressId = existingHouse.AddressId;
            houseToUpdate.VillageId = existingHouse.VillageId;
            houseToUpdate.LandlordId = existingHouse.LandlordId;
            houseToUpdate.CampusId = existingHouse.CampusId;
            houseToUpdate.DistanceToCampus = existingHouse.DistanceToCampus;
            houseToUpdate.PowerPrice = existingHouse.PowerPrice;
            houseToUpdate.WaterPrice = existingHouse.WaterPrice;
            houseToUpdate.FingerprintLock = existingHouse.FingerprintLock;
            houseToUpdate.Camera = existingHouse.Camera;
            houseToUpdate.Parking = existingHouse.Parking;
            houseToUpdate.Deleted = existingHouse.Deleted;
            houseToUpdate.CreatedDate = existingHouse.CreatedDate;
            houseToUpdate.LastModifiedDate = existingHouse.LastModifiedDate;
            houseToUpdate.CreatedBy = existingHouse.CreatedBy;
            houseToUpdate.LastModifiedBy = existingHouse.LastModifiedBy;

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
            var houseController = new HouseController();
            int houseId = 2;

            //ACT

            //Get existing House
            var data = houseController.GetHouseById(houseId);
            //Using FluentAssertion to convert result data: from IActionResult to DTO/Model
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            HouseDTO existingHouse = okResult.Value.Should().BeAssignableTo<HouseDTO>().Subject;

            //Update House
            House houseToUpdate = new House();
            houseToUpdate.HouseId = -1; //update id (not exist)
            houseToUpdate.HouseName = existingHouse.HouseName;   
            houseToUpdate.View = existingHouse.View;
            houseToUpdate.Information = existingHouse.Information;
            houseToUpdate.AddressId = existingHouse.AddressId;
            houseToUpdate.VillageId = existingHouse.VillageId;
            houseToUpdate.LandlordId = existingHouse.LandlordId;
            houseToUpdate.CampusId = existingHouse.CampusId;
            houseToUpdate.DistanceToCampus = existingHouse.DistanceToCampus;
            houseToUpdate.PowerPrice = existingHouse.PowerPrice;
            houseToUpdate.WaterPrice = existingHouse.WaterPrice;
            houseToUpdate.FingerprintLock = existingHouse.FingerprintLock;
            houseToUpdate.Camera = existingHouse.Camera;
            houseToUpdate.Parking = existingHouse.Parking;
            houseToUpdate.Deleted = existingHouse.Deleted;
            houseToUpdate.CreatedDate = existingHouse.CreatedDate;
            houseToUpdate.LastModifiedDate = existingHouse.LastModifiedDate;
            houseToUpdate.CreatedBy = existingHouse.CreatedBy;
            houseToUpdate.LastModifiedBy = existingHouse.LastModifiedBy;

            var updatedData = houseController.UpdateHouse(houseToUpdate);

            //ASSERT
            Assert.IsInstanceOf<BadRequestObjectResult>(updatedData);
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
            var houseController = new HouseController();
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
            var houseController = new HouseController();

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
            var houseController = new HouseController();
            int houseId = 1;

            //ACT
            var data = houseController.IncreaseView(houseId);

            //ASSERT
            Assert.IsInstanceOf<OkResult>(data);
        }

        /**
         * Method: IncreaseView()
         * Scenario: Input HouseId: -1, 0, 1000 (invalid)
         * Expected behavior: Returns OkResult
         */
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1000)]
        public void IncreaseView_InvalidId_Returns_NotFoundResult(int houseId)
        {
            //ARRANGE
            var houseController = new HouseController();

            //ACT
            var data = houseController.IncreaseView(houseId);

            //ASSERT
            Assert.IsInstanceOf<NotFoundResult>(data);
        }

        #endregion IncreaseView

    }
}
