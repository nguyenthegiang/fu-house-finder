using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class RateDTO
    {
        public int RateId { get; set; }
        public int? Star { get; set; }
        public string Comment { get; set; }
        public string LandlordReply { get; set; }
        public int HouseId { get; set; }
        public string StudentId { get; set; }

        //No need for deleted because we always only query undeleted data
        //public bool Deleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
