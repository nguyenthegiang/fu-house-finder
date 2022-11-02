using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class UserRole
    {
        public UserRole()
        {
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
