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

                    //Get list of active or inactive landlords, order by name
                    landlords = context.Users.
                        ProjectTo<UserDTO>(config).
                        Where(u => u.Role.RoleName.Equals("Landlord")).
                        Where(u => u.StatusId == 0 || u.StatusId == 1).
                        OrderBy(landlord => landlord.DisplayName).
                        ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return landlords;
        }

        //[Staff][Landlord Sign up Request] Get list of landlords 
        public static List<UserDTO> GetRejectedLandlords()
        {
            List<UserDTO> landlords;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));

                    //Get list of active or inactive landlords, order by name
                    landlords = context.Users.
                        ProjectTo<UserDTO>(config).
                        Where(u => u.Role.RoleName.Equals("Landlord")).
                        Where(u => u.StatusId == 3).
                        OrderBy(landlord => landlord.DisplayName).
                        ToList();
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

        /**
         * [Login] Login with Email & Password
         */
        public static ResponseDTO LoginUsername(string email, string password)
        {
            ResponseDTO userDTO;
            using (var context = new FUHouseFinderContext())
            {
                //Get by Id, include Address
                //Check Email
                MapperConfiguration config;
                config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                User user = context.Users.Where(u => u.Email.Equals(email))
                    .Include(u => u.Address).Include(u => u.Role).FirstOrDefault();

                //Check Password (Hashing)
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

        /**
         * [Login] Login with Facebook
         */
        public static ResponseDTO LoginFacebook(string fid)
        {
            ResponseDTO userDTO;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get by Id, include Address
                    //Check FacebookId equals
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

        /**
         * [Login] Login with Google
         */
        public static ResponseDTO LoginGoogle(string gid)
        {
            ResponseDTO userDTO;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get by Id, include Address
                    //Check GoogleId equals
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

        /**
         * [Login] Register
         * Create new account for Student/Landlord
         */
        public static ResponseDTO Register(string fid, string gid, string email, string name, int role, string identityCardFrontSideImageLink, string identityCardBackSideImageLink, string phonenumber, string facebookUrl)
        {
            ResponseDTO userDTO;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Check if user has existed, if not => create account
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    userDTO = context.Users.Where(u => u.FacebookUserId.Equals(fid) && u.GoogleUserId.Equals(gid))
                        .Include(u => u.Address).Include(u => u.Role).ProjectTo<ResponseDTO>(config).FirstOrDefault();

                    if (userDTO == null)
                    {
                        //Make UserId
                        var lastUser = context.Users.Where(u => u.RoleId == role).OrderBy(u => u.UserId).LastOrDefault();
                        int index = 0;
                        if (lastUser != null)
                        {
                            index = Int32.Parse(lastUser.UserId.Substring(2)) + 1;  //id auto increment
                        }
                        string userPrefix = role == 1 ? "HE" : "LA";    //Prefix indicated by role

                        User user = new User();
                        user.UserId = userPrefix + index.ToString("D6");
                        user.FacebookUserId = fid;
                        user.GoogleUserId = gid;
                        user.Email = email;
                        user.DisplayName = name;
                        user.IdentityCardFrontSideImageLink = identityCardFrontSideImageLink;
                        user.IdentityCardBackSideImageLink = identityCardBackSideImageLink;
                        user.FacebookUrl = facebookUrl;
                        user.PhoneNumber = phonenumber;
                        user.RoleId = role;
                        user.Password = "";     //No password because login with fb/gg
                        user.CreatedBy = user.UserId;
                        user.CreatedDate = DateTime.UtcNow;
                        user.LastModifiedBy = user.UserId;
                        user.LastModifiedDate = DateTime.UtcNow;
                        int status = role == 2 ? 2 : 1;     //Landlord is disabled when created (Student is not)
                        user.StatusId = status;

                        context.Users.Add(user);
                        context.SaveChanges();

                        //Get user to return
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
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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

        /**
         * [Staff/list-landlord-signup-request]
         * Get List of Landlords waiting to sign up
         */
        public static List<UserDTO> GetLandlordSignupRequest()
        {
            List<UserDTO> landlords;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));

                    //Get list of landlord waiting to sign up, order by name
                    landlords = context.Users.
                        ProjectTo<UserDTO>(config).
                        Where(u => u.Role.RoleName.Equals("Landlord")).
                        Where(u => u.StatusId == 2).
                        OrderBy(landlord => landlord.DisplayName).
                        ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return landlords;
        }

        public static void UpdateUserStatus(string userId, int statusId, string staffId)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get user for update
                    User updateUser = context.Users.FirstOrDefault(u => u.UserId == userId);

                    //Update properties
                    updateUser.StatusId = statusId;
                    updateUser.LastModifiedBy = staffId;
                    updateUser.LastModifiedDate = DateTime.Today;

                    context.Entry<User>(updateUser).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //[Admin][Dashboard] Get list of staffs 
        public static List<UserDTO> GetStaffs()
        {
            List<UserDTO> staffs;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    staffs = context.Users.ProjectTo<UserDTO>(config).Where(u => !u.Role.RoleName.Equals("Landlord") && !u.Role.RoleName.Equals("Student")).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return staffs;
        }

        public static void UpdateProfile(string userId, string name, string email)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Find rooms of this house
                    User user = context.Users.FirstOrDefault(u => u.UserId.Equals(userId));
                    if (user == null)
                    {
                        throw new Exception();
                    }

                    //Update
                    user.DisplayName = name;
                    user.Email = email;
                    context.Users.Update(user);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void ChangePassword(string userId, string newPassword)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Find user with user id
                    User user = context.Users.FirstOrDefault(u => u.UserId.Equals(userId));
                    if (user == null)
                    {
                        throw new Exception();
                    }

                    //Update
                    PasswordHasher<User> pw = new PasswordHasher<User>();
                    var password = pw.HashPassword(user, newPassword);
                    user.Password = password;
                    context.Users.Update(user);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Boolean CheckCurrentPassword(string userId, string curPassword)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Find user with user id
                    User user = context.Users.FirstOrDefault(u => u.UserId.Equals(userId));
                    if (user == null)
                    {
                        throw new Exception();
                    }

                    //Update
                    PasswordHasher<User> pw = new PasswordHasher<User>();
                    var result = pw.VerifyHashedPassword(user, user.Password, curPassword);
                    if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static void CreateStaffAccount(StaffAccountCreateDTO staff)
        {
            try
            {
                using(var context = new FUHouseFinderContext())
                {
                    User user = new User();
                    var lastUser = context.Users.Where(u => u.RoleId != 1 || u.RoleId != 2).OrderBy(u => u.UserId).LastOrDefault();
                    int index = 0;
                    if (lastUser != null)
                    {
                        index = Int32.Parse(lastUser.UserId.Substring(2)) + 1;  //id auto increment
                    }
                    user.UserId = "SA" + index.ToString("D6");
                    user.Email = staff.Email;
                    user.DisplayName = staff.DisplayName;
                    user.RoleId = staff.Role;
                    user.CreatedDate = DateTime.Now;
                    user.LastModifiedDate = DateTime.Now;
                    PasswordHasher<User> pw = new PasswordHasher<User>();
                    var password = pw.HashPassword(user, staff.Password);
                    user.Password = password;
                    user.StatusId = 1;
                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void UpdateStaffAccount(StaffAccountUpdateDTO staff)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    User user = context.Users.Where(u => u.UserId == staff.Uid).FirstOrDefault();
                    if (user == null) return;
                    user.DisplayName = staff.DisplayName;
                    user.Email = staff.Email;
                    user.RoleId = staff.Role;
                    user.LastModifiedDate = DateTime.Now;
                    context.Users.Update(user);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void DeleteStaffAccount(string uid)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    User user = context.Users.Where(u => u.UserId == uid).FirstOrDefault();
                    if (user == null) return;
                    context.Users.Remove(user);
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
