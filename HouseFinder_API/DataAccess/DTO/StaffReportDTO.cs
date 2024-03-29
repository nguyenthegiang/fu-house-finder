﻿using BusinessObjects;
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
        public virtual ReportHouseDTO House { get; set; }
        public virtual UserDTO Student { get; set; }
        public ReportStatus Status { get; set; }
        public bool Deleted { get; set; }
        public DateTime ReportedDate { get; set; }
        public DateTime? SolvedDate { get; set; }
        public virtual StaffDTO SolvedByNavigation { get; set; }
    }
}
