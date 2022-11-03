using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Status
    {
        public Status()
        {
            Rooms = new HashSet<Room>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
