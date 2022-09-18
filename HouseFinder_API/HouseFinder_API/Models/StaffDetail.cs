using System;
using System.Collections.Generic;

#nullable disable

namespace HouseFinder_API.Models
{
    public partial class StaffDetail
    {
        public string StaffId { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }

        public virtual StaffDepartment Department { get; set; }
        public virtual StaffPosition Position { get; set; }
        public virtual User Staff { get; set; }
    }
}
