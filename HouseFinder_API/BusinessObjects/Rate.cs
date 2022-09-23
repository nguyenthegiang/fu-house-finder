using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Rate
    {
        public int RateId { get; set; }
        public int? Star { get; set; }
        public string Comment { get; set; }
        public string LandlordReply { get; set; }
        public int? HouseId { get; set; }
        public string StudentId { get; set; }

        public virtual House House { get; set; }
        public virtual User Student { get; set; }
    }
}
