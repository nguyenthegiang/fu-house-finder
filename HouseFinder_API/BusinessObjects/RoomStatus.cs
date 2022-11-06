using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class RoomStatus
    {
        public RoomStatus()
        {
            Rooms = new HashSet<Room>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
