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
                room.CreatedDate = DateTime.Now;
                room.LastModifiedDate = DateTime.Now;
                roomRepository.CreateRoom(room);
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

                roomRepository.UpdateRoomByRoomId(updatedRoom);
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
                roomRepository.DeleteRoom(roomId);
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


        //GET: api/Rooms/CountTotalHouse
        [HttpGet("CountAvailableCapacity")]
        public int? CountAvailableCapacity()
        {
            int? capacity = roomRepository.CountAvailableCapacity();
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

        [HttpPut("changeStatus")]
        public IActionResult ChangStatusRoom(int statusId, int roomId)
        {
            try
            {

                roomRepository.ChangStatusRoom(statusId, roomId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


    }
}
