using System;
using System.Collections.Generic;

#nullable disable

namespace HouseFinder_API.Models
{
    public partial class Status
    {
        public Status()
        {
            Rooms = new HashSet<Room>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
