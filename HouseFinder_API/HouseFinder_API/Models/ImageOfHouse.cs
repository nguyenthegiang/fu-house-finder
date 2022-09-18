using System;
using System.Collections.Generic;

#nullable disable

namespace HouseFinder_API.Models
{
    public partial class ImageOfHouse
    {
        public int ImageId { get; set; }
        public string ImageLink { get; set; }
        public int? HouseId { get; set; }

        public virtual House House { get; set; }
    }
}
