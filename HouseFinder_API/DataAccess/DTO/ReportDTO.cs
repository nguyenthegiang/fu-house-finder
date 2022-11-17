using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class ReportDTO
    {
        public int ReportId { get; set; }
        public string ReportContent { get; set; }
        public string StudentId { get; set; }
        public int HouseId { get; set; }
        public int StatusId { get; set; }
        public bool Deleted { get; set; }
        public DateTime ReportedDate { get; set; }
        public DateTime? SolvedDate { get; set; }
        public string SolvedBy { get; set; }
    }
}
