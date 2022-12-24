using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IUserRepository
    {
        UserDTO GetUserByID(string UserId);
        public ResponseDTO Login(LoginDTO login);
        public ResponseDTO Register(RegisterDTO register);
        public List<UserDTO> GetLandlords();
        public int CountTotalLandlord();
        public void UpdateUserIdCardImage(UserDTO user);
        public int CountActiveLandlord();
        public int CountInactiveLandlord();
        public List<UserDTO> GetLandlordSignupRequest();
        public void UpdateUserStatus(string userId, int statusId, string staffId);
        public List<UserDTO> GetStaffs();
        public void UpdateProfile(string userId, string name, string email);
        public void LandLordUpdateProfile(string userId, string name, string phoneNumber, string facebookUrl);
        public void ChangePassword(string userId, string newPassword);
        public void CreateStaffAccount(StaffAccountCreateDTO staff);
        public void UpdateStaffAccount(StaffAccountUpdateDTO staff);
        public void DeleteStaffAccount(string uid);
        public Boolean CheckOldPassword(string userId, string oldPassword);
        public List<UserDTO> GetRejectedLandlords();
    }
}
