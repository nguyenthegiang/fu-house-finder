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
        public int? HouseId { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual House House { get; set; }
        public virtual User LastModifiedByNavigation { get; set; }
        public virtual User Student { get; set; }
    }
}
