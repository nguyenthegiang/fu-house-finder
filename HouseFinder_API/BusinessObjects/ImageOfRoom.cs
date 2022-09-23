using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class ImageOfRoom
    {
        public int ImageId { get; set; }
        public string ImageLink { get; set; }
        public int? RoomId { get; set; }

        public virtual Room Room { get; set; }
    }
}
