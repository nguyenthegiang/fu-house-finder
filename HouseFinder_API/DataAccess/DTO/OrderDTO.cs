using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string OrderContent { get; set; }
        public bool? Solved { get; set; }
        public DateTime? OrderedDate { get; set; }
        public DateTime? SolvedDate { get; set; }

    }
}
