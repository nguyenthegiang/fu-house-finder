using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Report
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

        public virtual House House { get; set; }
        public virtual User SolvedByNavigation { get; set; }
        public virtual ReportStatus Status { get; set; }
        public virtual User Student { get; set; }
    }
}
