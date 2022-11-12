using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class ReportHouseDTO
    {
        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public virtual AddressDTO Address { get; set; }
        public virtual UserDTO Landlord { get; set; }
        public int NumberOfReport { get; set; }
        public virtual List<StaffReportDTO> ListReports { get; set; }
    }
}
