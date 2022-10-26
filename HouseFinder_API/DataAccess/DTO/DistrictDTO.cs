using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class DistrictDTO
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public DateTime? CreatedDate { get; set; }

        //List Commune of this DTO
        public virtual ICollection<CommuneDTO> Communes { get; set; }
    }
}
