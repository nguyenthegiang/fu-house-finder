﻿using BusinessObjects;
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
    public class RoomsController : ControllerBase
    {
        private IRoomsRepository roomsRepository = new RoomRepository();

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
        public IActionResult UpdateRoomByRoomId(Room room)
        {
            try
            {
                room.LastModifiedDate = DateTime.Now;
                roomsRepository.UpdateRoomByRoomId(room);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        //DELETE: api/Rooms?roomId=
        [HttpDelete]
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
    }
}
