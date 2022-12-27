using BusinessObjects;
using DataAccess;
using DataAccess.DTO;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class RoomImageRepository : IRoomImageRepository
    {
        public void CreateRoomImages(List<ImagesOfRoom> images)
        {
            try
            {
                ImageOfRoomDAO.CreateRoomImages(images);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        

        public ImagesOfRoomDTO GetImagesOfRoom(int id)
        {
            try
            {
                return ImageOfRoomDAO.GetRoomImage(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ImagesOfRoomDTO> GetRoomImages(int RoomId)
        {
            List<ImagesOfRoomDTO> images;
            try
            {
                images = ImageOfRoomDAO.GetRoomImages(RoomId);
            }
            catch (Exception)
            {
                images = new List<ImagesOfRoomDTO>();
            }
            return images;
        }

        public void UpdateRoomImages(ImagesOfRoomDTO images)
        {
            try
            {
                ImagesOfRoom image = new ImagesOfRoom();
                image.ImageId = images.ImageId;
                image.CreatedBy = images.CreatedBy;
                image.CreatedDate = images.CreatedDate;
                image.LastModifiedBy = images.LastModifiedBy;
                image.LastModifiedDate = DateTime.Now;
                image.RoomId = images.RoomId;
                image.ImageLink = images.ImageLink;
                image.Deleted = images.Deleted;
                ImageOfRoomDAO.UpdateRoomImage(image);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
