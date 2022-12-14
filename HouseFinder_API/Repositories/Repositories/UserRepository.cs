using BusinessObjects;
using DataAccess;
using DataAccess.DTO;
using Microsoft.AspNetCore.Identity;
using Repositories.IRepository;
using System;
using System.Collections.Generic;

namespace Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        public int CountActiveLandlord() => UserDAO.CountActiveLandlord();

        public int CountInactiveLandlord() => UserDAO.CountInactiveLandlord();

        public int CountTotalLandlord() => UserDAO.CountTotalLandlord();

        //Get list of landlords
        public List<UserDTO> GetLandlords() => UserDAO.GetLandlords();

        public List<UserDTO> GetLandlordSignupRequest() => UserDAO.GetLandlordSignupRequest();

        public UserDTO GetUserByID(string UserId) => UserDAO.GetUserById(UserId);

        public ResponseDTO Login(LoginDTO login)
        {
            ResponseDTO user;
            //Choosing method to login
            if (login.FacebookUserId != null)
            {
                user = UserDAO.LoginFacebook(login.FacebookUserId);
            }
            else if (login.GoogleUserId != null)
            {
                user = UserDAO.LoginGoogle(login.GoogleUserId);
            }
            else
            {
                user = UserDAO.LoginUsername(login.Email, login.Password);
            }
            return user;
        }

        public ResponseDTO Register(RegisterDTO register)
        {
            try
            {
                AddressDTO address = AddressDAO.CreateAddress(register.Address, "");
                if (register.RoleName == "student")
                {
                    register.RoleId = 1;
                }
                else if (register.RoleName == "landlord")
                {
                    register.RoleId = 2;
                }
                ResponseDTO user = UserDAO.Register(
                    register.FacebookUserId,
                    register.GoogleUserId,
                    register.Email,
                    register.DisplayName,
                    (int)register.RoleId,
                    register.IdentityCardFrontSideImageLink,
                    register.IdentityCardBackSideImageLink,
                    register.PhoneNumber,
                    register.FacebookUrl,
                    address.AddressId
                );
                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void UpdateUserIdCardImage(UserDTO user)
        {
            UserDAO.UpdateUserIdCardImage(user.UserId, user.IdentityCardFrontSideImageLink, user.IdentityCardBackSideImageLink);
        }
        public void UpdateUserStatus(string userId, int statusId, string staffId) => UserDAO.UpdateUserStatus(userId, statusId, staffId);

        //Get list of staffs
        public List<UserDTO> GetStaffs() => UserDAO.GetStaffs();
        public void UpdateProfile(string userId, string name, string email) => UserDAO.UpdateProfile(userId, name, email);
        public void LandLordUpdateProfile(string userId, string name, string phoneNumber, string facebookUrl) => UserDAO.LandLordUpdateProfile(userId, name, phoneNumber, facebookUrl);
        public void ChangePassword(string userId, string newPassword) => UserDAO.ChangePassword(userId, newPassword);

        public void CreateStaffAccount(StaffAccountCreateDTO staff) => UserDAO.CreateStaffAccount(staff);

        public Boolean CheckOldPassword(string userId, string oldPassword) => UserDAO.CheckCurrentPassword(userId, oldPassword);
        public List<UserDTO> GetRejectedLandlords() => UserDAO.GetRejectedLandlords();

        public void UpdateStaffAccount(StaffAccountUpdateDTO staff) => UserDAO.UpdateStaffAccount(staff);
        public void DeleteStaffAccount(string uid) => UserDAO.DeleteStaffAccount(uid);
    }
}
