using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public string StudentId { get; set; }
        public string OrderContent { get; set; }

        public virtual User Student { get; set; }
    }
}
