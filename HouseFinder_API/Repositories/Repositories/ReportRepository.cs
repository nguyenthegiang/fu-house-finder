using BusinessObjects;
using DataAccess;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class ReportRepository: IReportRepository
    {
        public void AddReport(Report report) => ReportDAO.AddReport(report);
    }
}
