using BusinessObjects;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IRoomImageRepository
    {
        public void CreateRoomImages(List<ImagesOfRoom> images);
        public List<ImagesOfRoomDTO> GetRoomImages(int RoomId);
    }
}
