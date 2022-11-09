﻿using BusinessObjects;
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

        public List<ReportDTO> GetAllReport() => ReportDAO.GetAllReport();

        public int[] GetTotalReportByMonth() => ReportDAO.GetTotalReportByMonth();
    }
}
