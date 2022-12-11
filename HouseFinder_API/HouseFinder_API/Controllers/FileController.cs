using BusinessObjects;
using DataAccess.DTO;
using HouseFinder_API.Helper;
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
        private IUserRepository userReposiotry = new UserRepository();
        private IRoomImageRepository roomImageRepository = new RoomImageRepository();
        private IHouseImageRepository houseImageRepository = new HouseImageRepository();

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
        [HttpPost("upload/{HouseId:int}")]
        public IActionResult UploadFile(IFormFile File, [FromRoute] int HouseId)
        {
            var uid = HttpContext.Session.GetString("User");
            if (String.IsNullOrEmpty(uid))
            {
                return Forbid();
            }

            Stream fs = File.OpenReadStream();
            if (!checkXlsxMimeType(File))
                return BadRequest("Invalid File Type");
            XSSFWorkbook wb = new XSSFWorkbook(fs);
            LoadData(wb, HouseId, uid);
            return Ok();
        }

        private void LoadData(XSSFWorkbook wb, int HouseId, string LandlordId)
        {
            List<string> errors = new List<string>();
            int row = 2;

            // CREATE ROOM RECORDS
            ISheet roomSheet = wb.GetSheetAt(0);
            List<Room> roomList = new List<Room>();
            while (true)
            {
                try
                {
                    var record = roomSheet.GetRow(row);
                    if (record == null)
                    {
                        break;
                    }
                    var _buildingNumber = record.GetCell(0).NumericCellValue;
                    var _floorNumber = record.GetCell(1).NumericCellValue;
                    var _roomName = record.GetCell(2).ToString();
                    var _roomPrice = record.GetCell(3).NumericCellValue;
                    var _roomArea = record.GetCell(4).NumericCellValue;
                    var _roomCapacity = record.GetCell(5).NumericCellValue;
                    var _currentPeople = record.GetCell(6).NumericCellValue;
                    var _roomType = record.GetCell(7).ToString();
                    var _information = record.GetCell(8).ToString();
                    var _fridge = record.GetCell(9).ToString();
                    var _kitchen = record.GetCell(10).ToString();
                    var _washingMachine = record.GetCell(11).ToString();
                    var _desk = record.GetCell(12).ToString();
                    var _bed = record.GetCell(13).ToString();
                    var _closedToilet = record.GetCell(14).ToString();
                    var _withHost = record.GetCell(15).ToString();

                    var fridge = _fridge.Equals("YES");
                    var kitchen = _kitchen.Equals("YES");
                    var washingMachine = _kitchen.Equals("YES");
                    var desk = _desk.Equals("YES");
                    var bed = _bed.Equals("YES");
                    var closedToilet = _closedToilet.Equals("YES");
                    var withHost = _withHost.Equals("YES");
                    int roomType;
                    if (_roomType.Equals("Chung cư mini"))
                    {
                        roomType = 3;
                    }
                    else if (_roomType.Equals("Không khép kín")) 
                    {
                        roomType = 2;
                    }
                    else
                    {
                        roomType = 1;
                    }

                    Room room = new Room();
                    room.BuildingNumber = (int)_buildingNumber;
                    room.FloorNumber = (int)_floorNumber;
                    room.RoomName = _roomName;
                    room.AreaByMeters = _roomArea;
                    room.PricePerMonth = (decimal)_roomPrice;
                    room.MaxAmountOfPeople = (int)_roomCapacity;
                    room.CurrentAmountOfPeople = (int)_currentPeople;
                    room.Fridge = fridge;
                    room.Kitchen = kitchen;
                    room.WashingMachine = washingMachine;
                    room.Desk = desk;
                    room.Bed = bed;
                    room.ClosedToilet = closedToilet;
                    room.NoLiveWithHost = withHost;
                    room.Information = _information;
                    room.HouseId = HouseId;
                    room.CreatedDate = DateTime.Now;
                    room.CreatedBy = LandlordId;
                    room.LastModifiedBy = LandlordId;
                    room.LastModifiedDate = DateTime.Now;
                    room.Deleted = false;
                    room.StatusId = _roomCapacity == _currentPeople ? 1 : 2;
                    room.RoomTypeId = roomType;
                    roomList.Add(room);
                }
                catch (Exception e)
                {
                    errors.Add($"Error at line {row}");
                    Console.WriteLine($"Error at line {row} --- {e.Message}");
                }

                row++;
            }
            roomsRepository.CreateRooms(roomList);
        }

        //[Authorize]
        [HttpPost("room/image")]
        public async Task<IActionResult> UploadRoomImage(IFormFile File, [ModelBinder(typeof(JsonModelBinder))] RoomImageInfoDTO Room)
        {
            string uid = HttpContext.Session.GetString("User");
            RoomDTO roomDTO = roomsRepository.GetRoomByHouseIdAndBuildingAndFloorAndRoomName(
                Room.HouseId,
                Room.BuildingNumber,
                Room.FloorNumber,
                Room.RoomName
                );
            if (roomDTO == null)
            {
                return NotFound("Room data for this image is not found!");
            }
            string dir = $"user/{uid}/House/{Room.HouseId}/{roomDTO.RoomId}";
            List<ImagesOfRoom> images = new List<ImagesOfRoom>();
            var path = $"{dir}/{File.FileName}";
            Stream fs = File.OpenReadStream();
            await storageRepository.UploadFileAsync(path, fs);
            ImagesOfRoom image = new ImagesOfRoom();
            image.CreatedBy = uid;
            image.CreatedDate = DateTime.UtcNow;
            image.LastModifiedBy = uid;
            image.RoomId = roomDTO.RoomId;
            image.ImageLink = path;
            images.Add(image);
            roomImageRepository.CreateRoomImages(images);
            return Ok();
        }

        [HttpPost("room/image/{RoomId:int}")]
        public async Task<IActionResult> UploadRoomImage(IFormFile File, [FromRoute] int RoomId)
        {
            string uid = HttpContext.Session.GetString("User");
            RoomDTO roomDTO = roomsRepository.GetRoomByRoomId(RoomId);
            if (roomDTO == null)
            {
                return NotFound("Room data for this image is not found!");
            }
            string dir = $"user/{uid}/House/{roomDTO.HouseId}/{RoomId}";
            List<ImagesOfRoom> images = new List<ImagesOfRoom>();
            var path = $"{dir}/{File.FileName}";
            Stream fs = File.OpenReadStream();
            await storageRepository.UploadFileAsync(path, fs);
            ImagesOfRoom image = new ImagesOfRoom();
            image.CreatedBy = uid;
            image.CreatedDate = DateTime.UtcNow;
            image.LastModifiedBy = uid;
            image.RoomId = roomDTO.RoomId;
            image.ImageLink = path;
            images.Add(image);
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

            //URL of Folder in Amazon S3 Server
            var dir = $"user/{user.UserId}/IDC".Trim();
            var frontImg = "";
            var backImg = "";

            //Upload identity card images (front & back) to Server
            for (int i = 0; i < 2; i++)
            {
                //Path to file in Server
                var path = $"{dir}/{files[i].FileName}";

                //File: an object of IFormFile
                Stream fs = files[i].OpenReadStream();

                //Upload to Server
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

        [Authorize]
        [HttpPost("house/image/{HouseId:int}")]
        public async Task<IActionResult> UploadHouseImage(List<IFormFile> files, [FromRoute] int HouseId)
        {
            string uid = HttpContext.Session.GetString("User");
            if (String.IsNullOrWhiteSpace(uid))
            {
                return Forbid();
            }
            string dir = $"user/{uid}/House/{HouseId}";
            foreach (var file in files)
            {
                var path = $"{dir}/{file.FileName}";
                Stream fs = file.OpenReadStream();
                await storageRepository.UploadFileAsync(path, fs);
                ImagesOfHouseDTO img = new ImagesOfHouseDTO();
                img.HouseId = HouseId;
                img.ImageLink = path;
                houseImageRepository.CreateHouseImage(img, uid);
            }
            return Ok();
        }
    }
}
