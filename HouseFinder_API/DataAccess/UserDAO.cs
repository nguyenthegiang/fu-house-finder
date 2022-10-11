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
        public static UserDTO GetUserById(string id)
        {
            UserDTO userDTO;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    userDTO = context.Users.Where(u => u.UserId.Equals(id)).Include(u => u.Address).ProjectTo<UserDTO>(config).FirstOrDefault();
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
