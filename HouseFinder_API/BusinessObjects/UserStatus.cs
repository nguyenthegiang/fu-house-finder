﻿using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class UserStatus
    {
        public UserStatus()
        {
            Users = new HashSet<User>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
