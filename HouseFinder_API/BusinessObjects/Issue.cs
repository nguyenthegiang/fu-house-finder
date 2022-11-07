using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Issue
    {
        public int IssueId { get; set; }
        public string Description { get; set; }
        public int RoomId { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        public virtual Room Room { get; set; }
    }
}
