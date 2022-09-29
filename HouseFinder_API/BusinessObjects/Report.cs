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
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }

        public virtual User CreatedUserNavigation { get; set; }
        public virtual House House { get; set; }
        public virtual User Student { get; set; }
        public virtual User UpdatedUserNavigation { get; set; }
    }
}
