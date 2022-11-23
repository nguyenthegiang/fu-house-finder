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
            CreateMap<RoomStatus, RoomStatusDTO>();
            CreateMap<User, ResponseDTO>().ForMember(des => des.RoleName, act => act.MapFrom(src => src.Role.RoleName));
            CreateMap<UserRole, RoleDTO>();
            CreateMap<RoomType, RoomTypeDTO>();
            CreateMap<Campus, CampusDTO>();
            CreateMap<ImagesOfHouse, ImagesOfHouseDTO>();
            CreateMap<Village, VillageDTO>();
            CreateMap<Commune, CommuneDTO>();
            CreateMap<District, DistrictDTO>();
            CreateMap<House, AvailableHouseDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<Rate, RateDTO>();
            CreateMap<OrderStatus, OrderStatusDTO>();
            CreateMap<Report, ReportDTO>();
            CreateMap<Report, StaffReportDTO>();
            CreateMap<House, ReportHouseDTO>();
            CreateMap<User, StaffDTO>();
        }
    }
}
