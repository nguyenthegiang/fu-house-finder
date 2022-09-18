using System;
using System.Collections.Generic;

#nullable disable

namespace HouseFinder_API.Models
{
    public partial class StaffPosition
    {
        public StaffPosition()
        {
            StaffDetails = new HashSet<StaffDetail>();
        }

        public int PositionId { get; set; }
        public string PositiontName { get; set; }

        public virtual ICollection<StaffDetail> StaffDetails { get; set; }
    }
}
