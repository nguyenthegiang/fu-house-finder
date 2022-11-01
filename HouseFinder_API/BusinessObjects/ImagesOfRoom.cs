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
        public bool? Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User LastModifiedByNavigation { get; set; }
        public virtual Room Room { get; set; }
    }
}
