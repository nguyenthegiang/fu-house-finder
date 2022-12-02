using DataAccess;
using DataAccess.DTO;
using Microsoft.AspNetCore.Identity;
using Repositories.IRepository;
using System;
using System.Collections.Generic;

namespace Repositories.Repositories
{
    public class UserRepository : IUserReposiotry
    {
        public int CountActiveLandlord() => UserDAO.CountActiveLandlord();

        public int CountInactiveLandlord() => UserDAO.CountInactiveLandlord();

        public int CountTotalLandlord() => UserDAO.CountTotalLandlord();

        //Get list of landlords
        public List<UserDTO> GetLandlords() => UserDAO.GetLandlords();

        public List<UserDTO> GetLandlordSignupRequest() => UserDAO.GetLandlordSignupRequest();

        //Get list of staffs
        //public List<UserDTO> GetStaffs() => UserDAO.GetStaffs();

        public UserDTO GetUserByID(string UserId) => UserDAO.GetUserById(UserId);
        public ResponseDTO Login(LoginDTO login)
        {
            ResponseDTO user;
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
                    register.FacebookUrl
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
    }
}
