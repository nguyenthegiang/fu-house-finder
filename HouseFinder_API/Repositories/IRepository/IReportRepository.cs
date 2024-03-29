﻿using BusinessObjects;
using DataAccess.DTO;
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
        public List<StaffReportDTO> SearchReportByName(string key);
        public int CountReportByHouseId(int houseId);
        public List<StaffReportDTO> GetAllReports();
        public int CounTotalReport();
        public StaffReportDTO GetReportById(int reportId);
        public void UpdateReportStatus(int reportId, int statusId, string account);
    }
}
