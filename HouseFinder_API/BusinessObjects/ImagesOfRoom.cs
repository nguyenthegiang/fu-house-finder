using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class ImagesOfRoom
    {
        public int ImageId { get; set; }
        public string ImageLink { get; set; }
        public int? RoomId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }

        public virtual User CreatedUserNavigation { get; set; }
        public virtual Room Room { get; set; }
        public virtual User UpdatedUserNavigation { get; set; }
    }
}
