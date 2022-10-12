using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public bool? Active { get; set; }
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
    }
}
