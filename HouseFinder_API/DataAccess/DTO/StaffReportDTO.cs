using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class StaffReportDTO
    {
        public int ReportId { get; set; }
        public string ReportContent { get; set; }
        public int HouseId { get; set; }
        public virtual UserDTO Student { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
