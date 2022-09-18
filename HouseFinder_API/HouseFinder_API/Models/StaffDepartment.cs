using System;
using System.Collections.Generic;

#nullable disable

namespace HouseFinder_API.Models
{
    public partial class StaffDepartment
    {
        public StaffDepartment()
        {
            StaffDetails = new HashSet<StaffDetail>();
        }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public virtual ICollection<StaffDetail> StaffDetails { get; set; }
    }
}
