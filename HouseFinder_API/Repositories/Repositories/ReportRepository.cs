using BusinessObjects;
using DataAccess;
using DataAccess.DTO;
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

        public int CounTotalReport() => ReportDAO.CountTotalReport();

        public int CountReportByHouseId(int houseId) => ReportDAO.CountTotalReportByHouseId(houseId);

        public List<StaffReportDTO> GetAllReports() => ReportDAO.GetAllReports();

        public int[] GetTotalReportByMonth() => ReportDAO.GetTotalReportByMonth();

        public List<StaffReportDTO> SearchReportByName(string key) => ReportDAO.SearchReportByName(key);

        public StaffReportDTO GetReportById(int reportId) => ReportDAO.GetReportById(reportId);

        public void UpdateReportStatus(int reportId, int statusId, string account) => ReportDAO.UpdateReportStatus(reportId, statusId, account);
    }
}
