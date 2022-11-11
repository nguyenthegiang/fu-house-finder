using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class CampusDTO
    {
        public int CampusId { get; set; }
        public string CampusName { get; set; }

        //List District of this Campus
        public virtual ICollection<DistrictDTO> Districts { get; set; }

        //Address of this Campus (for Filter by Distance in Home Page)
        public virtual AddressDTO Address { get; set; }

    }
}
