using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string FacebookUserId { get; set; }
        public string GoogleUserId { get; set; }
        public string Email { get; set; }

        //Not show password when send API
        //public string Password { get; set; }

        public string DisplayName { get; set; }
        public int StatusId { get; set; }
        public string ProfileImageLink { get; set; }
        public string PhoneNumber { get; set; }
        public string FacebookUrl { get; set; }
        public string IdentityCardFrontSideImageLink { get; set; }
        public string IdentityCardBackSideImageLink { get; set; }
        public int? AddressId { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        //Address of this User
        public virtual AddressDTO Address { get; set; }
        public virtual RoleDTO Role { get; set; }
    }

    //UserDTO used for Registering
    public class RegisterDTO
    {
        public string FacebookUserId { get; set; }
        public string GoogleUserId { get; set; }
        public string GoogleIdToken { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FacebookUrl { get; set; }
        public string DisplayName { get; set; }
        public string IdentityCardFrontSideImageLink { get; set; }
        public string IdentityCardBackSideImageLink { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public string Address { get; set; } = "";
    }

    //UserDTO used for Login
    public class LoginDTO
    {
        public string FacebookUserId { get; set; } = null;
        public string GoogleUserId { get; set; } = null;
        public string Email { get; set; } = null;
        public string Password { get; set; } = null;
    }

    //UserDTO used in Response for Login request
    public class ResponseDTO
    {
        public string AccessToken { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
        public string FacebookUserId { get; set; }
        public string GoogleUserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string RoleName { get; set; }
        public int StatusId { get; set; }
    }

    public class StaffAccountCreateDTO
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
    }

    public class StaffAccountUpdateDTO
    {
        public string Uid { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
    }

    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
