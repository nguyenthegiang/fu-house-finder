using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObjects;
using DataAccess.DTO;
using Microsoft.AspNetCore.Identity;
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

        //[Staff][Dashboard] Get list of landlords 
         public static List<UserDTO> GetLandlords()
        {
            List<UserDTO> landlords;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    landlords = context.Users.ProjectTo<UserDTO>(config).Where(u => u.Role.RoleName.Equals("Landlord")).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return landlords;
        }

        //[Head][Dashboard] Get list of staffs
        //public static List<UserDTO> GetStaffs()
        //{
        //    List<UserDTO> staffs;
        //    try
        //    {
        //        using (var context = new FUHouseFinderContext())
        //        {
        //            MapperConfiguration config;
        //            config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
        //            staffs = context.Users.ProjectTo<UserDTO>(config).Where(u => u.Role.RoleName.Contains("Staff")).ToList();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //    return staffs;
        //}

        public static ResponseDTO LoginUsername(string email, string password)
        {
            ResponseDTO userDTO;
                using (var context = new FUHouseFinderContext())
                {
                    //Get by Id, include Address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    User user = context.Users.Where(u => u.Email.Equals(email))
                        .Include(u => u.Address).Include(u => u.Role).FirstOrDefault();

                    PasswordHasher<User> pw = new PasswordHasher<User>();
                    var result = pw.VerifyHashedPassword(user, user.Password, password);
                    if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        var mapper = config.CreateMapper();
                        userDTO = mapper.Map<ResponseDTO>(user);
                    }
                    else
                    {
                        userDTO = null;
                    }
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
                        .Include(u => u.Address).Include(u => u.Role).ProjectTo<ResponseDTO>(config).FirstOrDefault();
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
        public static ResponseDTO Register(string fid, string gid, string email, string name, int role, string identityCardFrontSideImageLink, string identityCardBackSideImageLink, string phonenumber, string facebookUrl)
        {
            ResponseDTO userDTO;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get by Id, include Address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    userDTO = context.Users.Where(u => u.FacebookUserId.Equals(fid) && u.GoogleUserId.Equals(gid))
                        .Include(u => u.Address).Include(u => u.Role).ProjectTo<ResponseDTO>(config).FirstOrDefault();
                    if (userDTO == null)
                    {
                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        Random random = new Random();

                        User user = new User();
                        user.UserId = new string(Enumerable.Repeat(chars, 10)
                            .Select(s => s[random.Next(s.Length)]).ToArray());

                        user.FacebookUserId = fid;
                        user.GoogleUserId = gid;
                        user.Email = email;
                        user.DisplayName = name;
                        user.IdentityCardFrontSideImageLink = identityCardFrontSideImageLink;
                        user.IdentityCardBackSideImageLink = identityCardBackSideImageLink;
                        user.FacebookUrl = facebookUrl;
                        user.PhoneNumber = phonenumber;
                        user.RoleId = role;
                        user.Password = "";
                        user.CreatedBy = user.UserId;
                        user.CreatedDate = DateTime.UtcNow;
                        user.LastModifiedBy = user.UserId;
                        user.LastModifiedDate = DateTime.UtcNow;
                        user.StatusId = 1;
                        context.Users.Add(user);
                        context.SaveChanges();
                        userDTO = context.Users.Where(u => u.UserId == user.UserId)
                            .Include(u => u.Role)
                            .ProjectTo<ResponseDTO>(config)
                            .FirstOrDefault();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return userDTO;
        }

        //[Staff/Dashboard] Count total landlords
        public static int CountTotalLandlord()
        {
            int total;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    total = context.Users.Where(u => u.Role.RoleName.Equals("Landlord")).Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return total;
        }

        public static void UpdateUserIdCardImage(string userId, string identityCardFrontSideImageLink, string identityCardBackSideImageLink)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    User user = context.Users.Where(u => u.UserId == userId).FirstOrDefault();
                    if (user != null)
                    {
                        user.IdentityCardFrontSideImageLink = identityCardFrontSideImageLink;
                        user.IdentityCardBackSideImageLink = identityCardBackSideImageLink;
                        context.Users.Update(user);
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        //[Staff/Dashboard] Count total active landlords
        public static int CountActiveLandlord()
        {
            int total;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    total = context.Users.Where(u => u.Role.RoleName.Equals("Landlord")).Where(l => l.StatusId == 1).Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return total;
        }

        //[Staff/Dashboard] Count total inactive landlords
        public static int CountInactiveLandlord()
        {
            int total;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    total = context.Users.Where(u => u.Role.RoleName.Equals("Landlord")).Where(l => l.StatusId == 2).Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return total;
        }

    }
}
