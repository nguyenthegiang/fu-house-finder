using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class VillageDTO
    {
        public int VillageId { get; set; }
        public string VillageName { get; set; }
        public int? CommuneId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
