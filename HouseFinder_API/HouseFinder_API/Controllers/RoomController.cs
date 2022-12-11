using BusinessObjects;
using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFinder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IRoomRepository roomRepository = new RoomRepository();

        //GET: api/Rooms/getByHouseId?HouseId=
        [HttpGet("getByHouseId")]
        public IActionResult GetRoomsByHouseId(int HouseId)
        {
            List<RoomDTO> roomsDTO = roomRepository.GetRoomsByHouseId(HouseId);
            if (roomsDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(roomsDTO);
            }
        }

        //GET: api/Rooms/getAvailableRooms?HouseId=
        [HttpGet("getAvailableRooms")]
        public IActionResult GetAvailableRoomsByHouseId(int HouseId)
        {
            List<RoomDTO> rooms = roomRepository.GetAvailableRoomsByHouseId(HouseId);
            if (rooms == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rooms);
            }
        }
        //GET: api/Rooms/getByRoomId?RoomId=
        [HttpGet("getByRoomId")]
        public IActionResult GetRoomsByRoomId(int RoomId)
        {
            RoomDTO roomsDTO = roomRepository.GetRoomByRoomId(RoomId);
            if (roomsDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(roomsDTO);
            }
        }

        //POST: api/Rooms
        [HttpPost]
        public IActionResult CreateRoom(Room room)
        {
            try
            {
                Room createRoom = new Room();
                createRoom.RoomName = room.RoomName;
                createRoom.PricePerMonth = (decimal)room.PricePerMonth;
                createRoom.Information = room.Information;
                createRoom.AreaByMeters = room.AreaByMeters;
                createRoom.Fridge = room.Fridge;
                createRoom.Kitchen = room.Kitchen;
                createRoom.WashingMachine = room.WashingMachine;
                createRoom.Desk = room.Desk;
                createRoom.NoLiveWithHost = room.NoLiveWithHost;
                createRoom.Bed = room.Bed;
                createRoom.ClosedToilet = room.ClosedToilet;
                createRoom.MaxAmountOfPeople = room.MaxAmountOfPeople;
                createRoom.CurrentAmountOfPeople = room.CurrentAmountOfPeople;
                createRoom.BuildingNumber = room.BuildingNumber;
                createRoom.FloorNumber = room.FloorNumber;
                createRoom.StatusId = room.Status.StatusId;
                createRoom.RoomTypeId = (int)room.RoomType.RoomTypeId;
                createRoom.HouseId = (int)room.HouseId;
                createRoom.Deleted = (bool)room.Deleted;
                createRoom.CreatedDate = DateTime.Now;
                createRoom.LastModifiedDate = DateTime.Now;
                createRoom.LastModifiedBy = room.LastModifiedBy;
                createRoom.CreatedBy = room.CreatedBy;
            
                roomRepository.CreateRoom(createRoom);
                return Ok(new { Status = 200 });
            }
            catch (Exception)
            {
                return Ok(new { Status = 400 });
            }
        }
        [HttpPost("create")]
        public IActionResult CreateRoom(CreateRoomDTO room)
        {
            try
            {
                string uid = HttpContext.Session.GetString("User");
                if (String.IsNullOrEmpty(uid))
                {
                    return Forbid();
                }
                Room createRoom = new Room();
                createRoom.RoomName = room.RoomName;
                createRoom.PricePerMonth = (decimal)room.PricePerMonth;
                createRoom.Information = room.Information;
                createRoom.AreaByMeters = room.AreaByMeters;
                createRoom.Fridge = room.Fridge;
                createRoom.Kitchen = room.Kitchen;
                createRoom.WashingMachine = room.WashingMachine;
                createRoom.Desk = room.Desk;
                createRoom.NoLiveWithHost = room.NoLiveWithHost;
                createRoom.Bed = room.Bed;
                createRoom.ClosedToilet = room.ClosedToilet;
                createRoom.MaxAmountOfPeople = room.MaxAmountOfPeople;
                createRoom.CurrentAmountOfPeople = room.CurrentAmountOfPeople;
                createRoom.BuildingNumber = room.BuildingNumber;
                createRoom.FloorNumber = room.FloorNumber;
                createRoom.RoomTypeId = (int)room.RoomTypeId;
                createRoom.HouseId = (int)room.HouseId;
                createRoom.Deleted = false;
                createRoom.CreatedDate = DateTime.Now;
                createRoom.LastModifiedDate = DateTime.Now;
                createRoom.LastModifiedBy = uid;
                createRoom.CreatedBy = uid;
                createRoom.CreatedDate = DateTime.Now;
                createRoom.StatusId = room.MaxAmountOfPeople == room.CurrentAmountOfPeople ? 1 : 2;

                RoomDTO roomDTO = roomRepository.CreateRoom(createRoom);
                return Ok(roomDTO);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //PUT: api/Rooms
        [HttpPut]
        public IActionResult UpdateRoom(RoomDTO roomDTO)
        {
            try
            {
                Room updatedRoom = new Room();
                updatedRoom.RoomId = roomDTO.RoomId;
                updatedRoom.RoomName = roomDTO.RoomName;
                updatedRoom.PricePerMonth = (decimal)roomDTO.PricePerMonth;
                updatedRoom.Information = roomDTO.Information;
                updatedRoom.AreaByMeters = roomDTO.AreaByMeters;
                updatedRoom.Fridge = roomDTO.Fridge;
                updatedRoom.Kitchen = roomDTO.Kitchen;
                updatedRoom.WashingMachine = roomDTO.WashingMachine;
                updatedRoom.Desk = roomDTO.Desk;
                updatedRoom.NoLiveWithHost = roomDTO.NoLiveWithHost;
                updatedRoom.Bed = roomDTO.Bed;
                updatedRoom.ClosedToilet = roomDTO.ClosedToilet;
                updatedRoom.MaxAmountOfPeople = roomDTO.MaxAmountOfPeople;
                updatedRoom.CurrentAmountOfPeople = roomDTO.CurrentAmountOfPeople;
                updatedRoom.BuildingNumber = roomDTO.BuildingNumber;
                updatedRoom.FloorNumber = roomDTO.FloorNumber;
                updatedRoom.StatusId = roomDTO.StatusId;
                updatedRoom.RoomTypeId = (int)roomDTO.RoomTypeId;
                updatedRoom.HouseId = (int)roomDTO.HouseId;
                updatedRoom.Deleted = (bool)roomDTO.Deleted;
                updatedRoom.CreatedDate = (DateTime)roomDTO.CreatedDate;
                updatedRoom.LastModifiedDate = roomDTO.LastModifiedDate;
                updatedRoom.LastModifiedBy = roomDTO.LastModifiedBy;
                updatedRoom.CreatedBy = roomDTO.CreatedBy;

                updatedRoom.LastModifiedDate = DateTime.Now;

                roomRepository.UpdateRoom(updatedRoom);
                return Ok(new { Status = 200 });
            }
            catch (Exception)
            {
                return Ok(new { Status = 400 });
            }
        }

        //DELETE: api/Rooms?roomId=
        [HttpDelete("Rooms")]
        public IActionResult DeleteRoom(int roomId)
        {
            try
            {
                roomRepository.DeleteRoom(roomId);
                return Ok(new { Status = 200 });
            }
            catch (Exception)
            {
                return BadRequest(new { Status = 400 });
            }
        }

        //GET: api/Rooms/CountTotalHouse
        [HttpGet("CountAvailableRoom")]
        public int CountAvailableRoom()
        {
            int availableRoom = roomRepository.CountAvailableRoom();
            return availableRoom;
        }

        //GET: api/Rooms/CountTotalRoom
        [HttpGet("CountTotalRoom")]
        public int CountTotalRoom()
        {
            int totalRoom = roomRepository.CountTotalRoom();
            return totalRoom;
        }


        //GET: api/Rooms/CountAvailableCapacity
        [HttpGet("CountAvailableCapacity")]
        public int? CountAvailableCapacity()
        {
            int? capacity = roomRepository.CountAvailableCapacity();
            return capacity;
        }

        //GET: api/Rooms/CountTotalCapacity
        [HttpGet("CountTotalCapacity")]
        public int? CountTotalCapacity()
        {
            int? capacity = roomRepository.CountTotalCapacity();
            return capacity;
        }

        //GET: api/Rooms/CountTotallyAvailableRoom
        [HttpGet("CountTotallyAvailableRoom")]
        public int? CountTotallyAvailableRoom()
        {
            int? capacity = roomRepository.CountTotallyAvailableRoom();
            return capacity;
        }

        //GET: api/Rooms/CountTotallyAvailableRoomByHouseId?houseId=
        [HttpGet("CountTotallyAvailableRoomByHouseId")]
        public int? CountTotallyAvailableRoomByHouseId(int houseId)
        {
            int? availableRoom = roomRepository.CountTotallyAvailableRoomByHouseId(houseId);
            return availableRoom;
        }

        //GET: api/Rooms/CountPartiallyAvailableRoomByHouseId?houseId=
        [HttpGet("CountPartiallyAvailableRoomByHouseId")]
        public int? CountPartiallyAvailableRoomByHouseId(int houseId)
        {
            int? availableRoom = roomRepository.CountPatiallyyAvailableRoomByHouseId(houseId);
            return availableRoom;
        }

        //GET: api/Rooms/CountAvailableCapacityByHouseId?houseId=
        [HttpGet("CountAvailableCapacityByHouseId")]
        public int? CountAvailableCapacityByHouseId(int houseId)
        {
            return roomRepository.CountAvailableCapacityByHouseId(houseId);
        }

        [HttpPut("changeStatus")]
        public IActionResult ChangeStatusRoom(int statusId, int roomId)
        {
            try
            {
                roomRepository.ChangeStatusRoom(statusId, roomId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
