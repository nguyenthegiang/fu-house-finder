using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Campus
    {
        public Campus()
        {
            Users = new HashSet<User>();
        }

        public int CampusId { get; set; }
        public string CampusName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
