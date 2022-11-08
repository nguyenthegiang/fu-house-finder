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
        private IRoomRepository roomsRepository = new RoomRepository();

        //GET: api/Rooms/getByHouseId?HouseId=
        [HttpGet("getByHouseId")]
        public IActionResult GetRoomsByHouseId(int HouseId)
        {
            List<RoomDTO> roomsDTO = roomsRepository.GetRoomsByHouseId(HouseId);
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
            List<RoomDTO> rooms = roomsRepository.GetAvailableRoomsByHouseId(HouseId);
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
            RoomDTO roomsDTO = roomsRepository.GetRoomByRoomId(RoomId);
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
                room.CreatedDate = DateTime.Now;
                room.LastModifiedDate = DateTime.Now;
                roomsRepository.CreateRoom(room);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //PUT: api/Rooms
        [HttpPut]
        public IActionResult UpdateRoomByRoomId(RoomDTO roomDTO)
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

                roomsRepository.UpdateRoomByRoomId(updatedRoom);
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        //DELETE: api/Rooms?roomId=
        [HttpDelete("Rooms")]
        public IActionResult DeleteRoom(int roomId)
        {
            try
            {
                roomsRepository.DeleteRoom(roomId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //GET: api/Rooms/CountTotalHouse
        [HttpGet("CountAvailableRoom")]
        public int CountAvailableRoom()
        {
            int availableRoom = roomsRepository.CountAvailableRoom();
            return availableRoom;
        }

        //GET: api/Rooms/CountTotalRoom
        [HttpGet("CountTotalRoom")]
        public int CountTotalRoom()
        {
            int totalRoom = roomsRepository.CountTotalRoom();
            return totalRoom;
        }


        //GET: api/Rooms/CountAvailableCapacity
        [HttpGet("CountAvailableCapacity")]
        public int? CountAvailableCapacity()
        {
            int? capacity = roomsRepository.CountAvailableCapacity();
            return capacity;
        }
        //GET: api/Rooms/CountTotalCapacity
        [HttpGet("CountTotalCapacity")]
        public int? CountTotalCapacity()
        {
            int? capacity = roomsRepository.CountTotalCapacity();
            return capacity;
        }


        //GET: api/Rooms/CountTotallyAvailableRoomByHouseId?houseId=
        [HttpGet("CountTotallyAvailableRoomByHouseId")]
        public int? CountTotallyAvailableRoomByHouseId(int houseId)
        {
            int? availableRoom = roomsRepository.CountTotallyAvailableRoomByHouseId(houseId);
            return availableRoom;
        }

        //GET: api/Rooms/CountPartiallyAvailableRoomByHouseId?houseId=
        [HttpGet("CountPartiallyAvailableRoomByHouseId")]
        public int? CountPartiallyAvailableRoomByHouseId(int houseId)
        {
            int? availableRoom = roomsRepository.CountPatiallyyAvailableRoomByHouseId(houseId);
            return availableRoom;
        }

        [HttpPut("changeStatus")]
        public IActionResult ChangStatusRoom(int statusId, int roomId)
        {
            try
            {

                roomsRepository.ChangStatusRoom(statusId, roomId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


    }
}
