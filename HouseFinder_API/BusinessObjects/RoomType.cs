using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class RoomType
    {
        public RoomType()
        {
            Rooms = new HashSet<Room>();
        }

        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
