using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObjects;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    }
}
