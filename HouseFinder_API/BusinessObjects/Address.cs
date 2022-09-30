using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public string Addresses { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
