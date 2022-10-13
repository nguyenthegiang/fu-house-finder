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

        public static ResponseDTO LoginUsername(string email, string password)
        {
            ResponseDTO userDTO;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get by Id, include Address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    userDTO = context.Users.Where(u => u.Email.Equals(email) && u.Password.Equals(password))
                        .Include(u => u.Address).Include(u => u.Role).ProjectTo<ResponseDTO>(config).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return userDTO;
        }
        public static ResponseDTO LoginFacebook(string fid)
        {
            ResponseDTO userDTO;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get by Id, include Address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    userDTO = context.Users.Where(u => u.FacebookUserId.Equals(fid))
                        .Include(u => u.Address).ProjectTo<ResponseDTO>(config).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return userDTO;
        }
        public static ResponseDTO LoginGoogle(string gid)
        {
            ResponseDTO userDTO;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get by Id, include Address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    userDTO = context.Users.Where(u => u.GoogleUserId.Equals(gid))
                        .Include(u => u.Address).Include(u => u.Role).ProjectTo<ResponseDTO>(config).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return userDTO;
        }
        public static ResponseDTO Register(string fid, string gid, string email, string name, int role)
        {
            ResponseDTO userDTO;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get by Id, include Address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    userDTO = context.Users.Where(u => u.FacebookUserId.Equals(fid))
                        .Include(u => u.Address).ProjectTo<ResponseDTO>(config).FirstOrDefault();
                    if (userDTO == null)
                    {
                        User user = new User();
                        user.FacebookUserId = fid;
                        user.GoogleUserId = gid;
                        user.Email = email;
                        user.DisplayName = name;
                        user.RoleId = role;
                        user.Password = "";
                        context.Users.Add(user);
                        userDTO = config.CreateMapper().Map<ResponseDTO>(user);
                    }
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
