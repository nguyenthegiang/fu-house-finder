using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class CommuneDTO
    {
        public int CommuneId { get; set; }
        public string CommuneName { get; set; }
        public int? DistrictId { get; set; }
        public DateTime? CreatedDate { get; set; }

        //List Villages of this Commune
        public virtual ICollection<VillageDTO> Villages { get; set; }

    }
}
