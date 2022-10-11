using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects;

namespace DataAccess.DTO
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<House, HouseDTO>();
            CreateMap<Address, AddressDTO>();
            CreateMap<Room, RoomDTO>();
            CreateMap<ImagesOfRoom, ImagesOfRoomDTO>();
            CreateMap<User, UserDTO>();
        }
    }
}
