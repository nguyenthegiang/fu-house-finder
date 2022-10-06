using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class ImagesOfHouse
    {
        public int ImageId { get; set; }
        public string ImageLink { get; set; }
        public int? HouseId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual House House { get; set; }
        public virtual User LastModifiedByNavigation { get; set; }
    }
}
