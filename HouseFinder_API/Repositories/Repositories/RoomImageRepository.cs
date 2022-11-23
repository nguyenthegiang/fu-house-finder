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
    }
}
