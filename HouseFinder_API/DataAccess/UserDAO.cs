using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObjects;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserDAO
    {
        //[House Detail] Get User detail information
        public static UserDTO GetUserById(string UserId)
        {
            UserDTO userDTO;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get by Id, include Address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    userDTO = context.Users.Where(u => u.UserId.Equals(UserId))
                        .Include(u => u.Address).ProjectTo<UserDTO>(config).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return userDTO;
        }
    }
}
