using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class AddressDTO
    {
        public int AddressId { get; set; }
        public string Addresses { get; set; }
        public string GoogleMapLocation { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
