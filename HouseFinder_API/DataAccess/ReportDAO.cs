using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObjects;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ReportDAO
    {
        public static void AddReport(Report report)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    context.Reports.Add(report);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //Count the total of report by month
        public static int[] GetTotalReportByMonth()
        {
            int[] totals = new int[12];
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get list order from the first date of this year to current date
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    List<ReportDTO> reports = context.Reports
                        .Where(r => r.Deleted == false)
                        .Where(o => o.ReportedDate.Year == DateTime.Today.Year)
                        .ProjectTo<ReportDTO>(config).ToList();

                    //Count total orders by month
                    for (int i = 0; i < 12; i++)
                    {
                        foreach (ReportDTO r in reports)
                        {
                            if (r.ReportedDate.Month == i+1)
                            {
                                totals[i]++;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return totals;
        }

        //Get list of reports by house id
        public static List<StaffReportDTO> GetReportByHouseId(int houseId)
        {
            List<StaffReportDTO> reports;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    reports = context.Reports.Where(r => r.Deleted == false).Where(r => r.HouseId == houseId).ProjectTo<StaffReportDTO>(config).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return reports;
        }

        //[Staff/list-report] Search report by house's name
        public static List<StaffReportDTO> SearchReportByName(string key)
        {
            List<StaffReportDTO> reports;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    reports = context.Reports.Where(r => r.Deleted == false).Where(r => r.House.HouseName.Contains(key)).ProjectTo<StaffReportDTO>(config).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return reports;
        }

        //[Staff/list-report] Count total report by house id
        public static int CountTotalReportByHouseId(int houseId)
        {
            int count;
            try
            {
                using(var context = new FUHouseFinderContext())
                {
                    count = context.Reports.Where(r => r.Deleted == false).Where(r => r.HouseId == houseId).Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return count;
        }

        //[Staff/list-report] Get all reports
        public static List<StaffReportDTO> GetAllReports()
        {
            List<StaffReportDTO> reports;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    reports = context.Reports.Where(r => r.Deleted != true).ProjectTo<StaffReportDTO>(config).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return reports;
        }

        //[Staff/list-report] Count total report
        public static int CountTotalReport()
        {
            int total;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    total = context.Reports.Where(r => r.Deleted != true).Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return total;
        }

        //[Staff/list-report] Get report by id
        public static StaffReportDTO GetReportById(int reportId)
        {
            StaffReportDTO report;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    report = context.Reports.Where(r => r.Deleted == false).ProjectTo<StaffReportDTO>(config)
                        .Where(r => r.ReportId == reportId).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return report;
        }
        //Update report status
        public static void UpdateReportStatus(int reportId, int statusId, string account)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    Report updateReport = context.Reports.FirstOrDefault(report => report.StatusId == reportId);
                    if (updateReport == null)
                    {
                        throw new Exception();
                    }
                    //Check status id
                    if (statusId == 1)
                    {
                        updateReport.SolvedDate = null;
                        updateReport.SolvedBy = null;
                    }
                    else if (statusId == 2)
                    {
                        updateReport.SolvedDate = null;
                        updateReport.SolvedBy = account;
                    }
                    else if (statusId == 3) //Add solved date and solved by for order if status change to solved
                    {
                        updateReport.SolvedDate = DateTime.Today;
                        updateReport.SolvedBy = account;
                    }
                    //Update order's status
                    updateReport.StatusId = statusId;

                    context.Entry<Report>(updateReport).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
