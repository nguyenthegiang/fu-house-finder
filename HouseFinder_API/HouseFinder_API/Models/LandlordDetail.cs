using System;
using System.Collections.Generic;

#nullable disable

namespace HouseFinder_API.Models
{
    public partial class LandlordDetail
    {
        public string LandlordId { get; set; }
        public string PhoneNumber { get; set; }
        public string FacebookUrl { get; set; }
        public string IdentityCardImageLink { get; set; }

        public virtual User Landlord { get; set; }
    }
}
