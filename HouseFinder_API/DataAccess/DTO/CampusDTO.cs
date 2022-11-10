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
        public int AddressId { get; set; }

        //List District of this Campus
        public virtual ICollection<DistrictDTO> Districts { get; set; }

    }
}
