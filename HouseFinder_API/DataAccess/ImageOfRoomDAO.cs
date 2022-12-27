using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObjects;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ImageOfRoomDAO
    {
        public static void CreateRoomImages(List<ImagesOfRoom> images)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    context.ImagesOfRooms.AddRange(images);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static List<ImagesOfRoomDTO> GetRoomImages(int RoomId)
        {
            List<ImagesOfRoomDTO> roomImages = new List<ImagesOfRoomDTO>();

            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    roomImages = context.ImagesOfRooms.Where(image => image.RoomId == RoomId)
                        .ProjectTo<ImagesOfRoomDTO>(config).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return roomImages;
        }
        public static ImagesOfRoomDTO GetRoomImage(int id)
        {
            ImagesOfRoomDTO roomImage = new ImagesOfRoomDTO();

            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    roomImage = context.ImagesOfRooms.Where(image => image.ImageId == id)
                        .ProjectTo<ImagesOfRoomDTO>(config).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return roomImage;
        }
        public static void UpdateRoomImage(ImagesOfRoom image)
        {

            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    context.ImagesOfRooms.Update(image);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}
