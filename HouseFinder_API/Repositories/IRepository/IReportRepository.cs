using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IReportRepository
    {
        public void AddReport(Report report);
        public int[] GetTotalReportByMonth();
    }
}
