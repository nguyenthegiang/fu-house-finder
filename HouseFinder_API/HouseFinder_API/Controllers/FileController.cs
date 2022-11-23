﻿using BusinessObjects;
using DataAccess.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Repositories;
using Repositories.IRepository;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HouseFinder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IWebHostEnvironment Environment;
        private IHouseRepository housesRepository = new HouseRepository();
        private IRoomRepository roomsRepository = new RoomRepository();
        private IUserReposiotry userReposiotry = new UserRepository();
        private IRoomImageRepository roomImageRepository = new RoomImageRepository();

        //Used for Upload file to Amazon S3 Server
        private readonly IStorageRepository storageRepository;

        //Inject through Constructor from Startup
        public FileController(IWebHostEnvironment _environment, IStorageRepository storageRepository)
        {
            Environment = _environment;
            this.storageRepository = storageRepository;
        }
        
        [HttpGet("download")]
        public IActionResult DownloadFile()
        {
            string filename = "house-template.xlsx";
            string filePath = Path.Combine(Environment.ContentRootPath, filename);
            Stream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            string mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(fs, mime, filename);
        }

        [Authorize]
        [HttpPost("upload")]
        public IActionResult UploadFile(IFormFile File)
        {
            Console.WriteLine($"called");
            Stream fs = File.OpenReadStream();
            if (!checkXlsxMimeType(File))
                return BadRequest("Invalid File Type");
            XSSFWorkbook wb = new XSSFWorkbook(fs);
            LoadDataAsync(wb);
            return Ok();
        }

        private void LoadDataAsync(XSSFWorkbook wb)
        {
            List<string> errors = new List<string>();
            ISheet houseSheet = wb.GetSheetAt(0);
            int row = 3;
            HouseDTO house = null;
            // CREATE HOUSE RECORD
            var houseRecord = houseSheet.GetRow(row);
            if (houseRecord != null)
            {
                var _campus = houseRecord.GetCell(0).StringCellValue;
                var _district = houseRecord.GetCell(1).StringCellValue;
                var _commune = houseRecord.GetCell(2).StringCellValue;
                var _village = houseRecord.GetCell(3).StringCellValue;
                var _address = houseRecord.GetCell(4).StringCellValue;
                var _googleAddress = houseRecord.GetCell(5).StringCellValue;
                var _houseName = houseRecord.GetCell(6).StringCellValue;
                var _houseInfo = houseRecord.GetCell(7).StringCellValue;
                var _powerPrice = houseRecord.GetCell(8).NumericCellValue;
                var _waterPrice = houseRecord.GetCell(9).NumericCellValue;
                var _fingerprint = houseRecord.GetCell(10).StringCellValue.Equals("YES");
                var _camera = houseRecord.GetCell(11).StringCellValue.Equals("YES");
                var _parking = houseRecord.GetCell(12).StringCellValue.Equals("YES");
                var _landlord = HttpContext.Session.GetString("User");
                house = housesRepository.CreateHouse(_houseName, _houseInfo, _address, _googleAddress, _village,
                    _landlord, _campus, (decimal)_powerPrice, (decimal)_waterPrice, _fingerprint, _camera, _parking);
            }

            // CREATE ROOM RECORDS
            ISheet roomSheet = wb.GetSheetAt(0);
            List<Room> roomList = new List<Room>();
            while (true)
            {
                try
                {
                    var record = roomSheet.GetRow(row);
                    row++;
                    if (record == null)
                    {
                        break;
                    }
                    var _buildingNumber = record.GetCell(0).NumericCellValue;
                    var _floorNumber = record.GetCell(1).NumericCellValue;
                    var _roomName = record.GetCell(2).StringCellValue;
                    var _roomPrice = record.GetCell(3).NumericCellValue;
                    var _roomArea = record.GetCell(4).NumericCellValue;
                    var _roomCapacity = record.GetCell(5).NumericCellValue;
                    var _information = record.GetCell(6).StringCellValue;
                    var _fridge = record.GetCell(7).StringCellValue;
                    var _kitchen = record.GetCell(8).StringCellValue;
                    var _washingMachine = record.GetCell(9).StringCellValue;
                    var _desk = record.GetCell(10).StringCellValue;
                    var _bed = record.GetCell(11).StringCellValue;
                    var _closedToilet = record.GetCell(12).StringCellValue;
                    var _withHost = record.GetCell(13).StringCellValue;
                    var fridge = _fridge.Equals("YES");
                    var kitchen = _kitchen.Equals("YES");
                    var washingMachine = _kitchen.Equals("YES");
                    var desk = _desk.Equals("YES");
                    var bed = _bed.Equals("YES");
                    var closedToilet = _closedToilet.Equals("YES");
                    var withHost = _withHost.Equals("YES");
                    Room room = new Room();
                    room.BuildingNumber = (int)_buildingNumber;
                    room.FloorNumber = (int)_floorNumber;
                    room.RoomName = _roomName;
                    room.AreaByMeters = _roomArea;
                    room.PricePerMonth = (decimal)_roomPrice;
                    room.MaxAmountOfPeople = (int)_roomCapacity;
                    room.Fridge = fridge;
                    room.Kitchen = kitchen;
                    room.WashingMachine = washingMachine;
                    room.Desk = desk;
                    room.Bed = bed;
                    room.ClosedToilet = closedToilet;
                    room.NoLiveWithHost = withHost;
                    room.Information = _information;
                    room.HouseId = house.HouseId;
                    room.CreatedDate = DateTime.UtcNow;
                    room.CreatedBy = house.LandlordId;
                    room.LastModifiedBy = house.LandlordId;
                    room.LastModifiedDate = DateTime.UtcNow;
                    room.Deleted = false;
                    roomList.Add(room);
                }
                catch (Exception)
                {
                    errors.Add($"Error at line {row}");
                }
            }
            roomsRepository.CreateRooms(roomList);
        }

        //[Authorize]
        [HttpPost("room/image")]
        public async Task<IActionResult> UploadRoomImage(List<IFormFile> files, [FromBody] RoomImageInfoDTO room)
        {
            string uid = HttpContext.Session.GetString("User");
            string dir = $"user/{uid}/House/{room.HouseId}/{room.RoomId}";
            List<ImagesOfRoom> images = new List<ImagesOfRoom>();
            for (int i=0; i < files.Count; i++)
            {
                var path = $"{dir}/{files[i].FileName}";
                Stream fs = files[i].OpenReadStream();
                await storageRepository.UploadFileAsync(path, fs);
                ImagesOfRoom image = new ImagesOfRoom();
                image.CreatedBy = uid;
                image.CreatedDate = DateTime.UtcNow;
                image.LastModifiedBy = uid;
                image.RoomId = room.RoomId;
                image.ImageLink = path;
                images.Add(image);
            }
            roomImageRepository.CreateRoomImages(images);
            return Ok();
        }

        private bool checkXlsxMimeType(IFormFile file)
        {
            string filename = file.FileName;
            string[] temp = filename.Split(".");
            string mimeType = temp[temp.Length - 1];
            return (
                mimeType.Equals("xlsx", StringComparison.OrdinalIgnoreCase)
                || mimeType.Equals("xls", StringComparison.OrdinalIgnoreCase)
            );
        }

        /**
         * [Landlord] Upload images of Identity Card
         */
        [Authorize]
        [HttpPost("idc/upload")]
        public async Task<IActionResult> UploadIDC(List<IFormFile> files)
        {
            //Get UserId from Session
            string uid = HttpContext.Session.GetString("User");
            if (uid == null)
            {
                return Forbid();
            }
            UserDTO user = userReposiotry.GetUserByID(uid);

            //URL of File in Amazon S3 Server
            var dir = $"user/{user.UserId}/IDC".Trim();
            var frontImg = "";
            var backImg = "";

            //Upload identity card images (front & back) to Server
            for (int i=0; i<2; i++)
            {
                var path = $"{dir}/{files[i].FileName}";
                Stream fs = files[i].OpenReadStream();
                await storageRepository.UploadFileAsync(path, fs);
                if (i == 0)
                    frontImg = path;
                else
                    backImg = path;
            }

            //Save URL to images uploaded
            user.IdentityCardFrontSideImageLink = frontImg;
            user.IdentityCardBackSideImageLink = backImg;
            userReposiotry.UpdateUserIdCardImage(user);

            return Ok();
        }
    }
}
