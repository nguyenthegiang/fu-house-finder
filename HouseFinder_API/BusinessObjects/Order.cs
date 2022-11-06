using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string OrderContent { get; set; }
        public int StatusId { get; set; }
        public DateTime OrderedDate { get; set; }
        public DateTime? SolvedDate { get; set; }

        public virtual OrderStatus Status { get; set; }
        public virtual User Student { get; set; }
    }
}
