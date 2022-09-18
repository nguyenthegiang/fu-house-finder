﻿using System;
using System.Collections.Generic;

#nullable disable

namespace HouseFinder_API.Models
{
    public partial class User
    {
        public User()
        {
            Houses = new HashSet<House>();
            Rates = new HashSet<Rate>();
        }

        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool? Active { get; set; }
        public int? RoleId { get; set; }
        public int? CampusId { get; set; }

        public virtual Campus Campus { get; set; }
        public virtual UserRole Role { get; set; }
        public virtual LandlordDetail LandlordDetail { get; set; }
        public virtual StaffDetail StaffDetail { get; set; }
        public virtual ICollection<House> Houses { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
    }
}
