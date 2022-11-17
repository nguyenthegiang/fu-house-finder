using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class ReportStatus
    {
        public ReportStatus()
        {
            Reports = new HashSet<Report>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}
