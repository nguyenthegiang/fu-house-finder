using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class ReportHouseDTO
    {
        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public int AddressId { get; set; }
        public virtual User Landlord { get; set; }
    }
}
