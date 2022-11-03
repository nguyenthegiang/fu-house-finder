using AutoMapper;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class OrderDAO
    {
        public int GetTotalOrderByMonth(int month)
        {
            int total = 0;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Convert month type int to string
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(month);
                    //Get list orders by month
                    List<Order> orders = context.Orders.Where(o => o.OrderedDate.ToString("MMMM dd").Contains(monthName)).ToList();
                    //Count number of orders
                    orders.Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return total;
        }


        //[Staff/Dashboard] Get total order of the current month
        public int GetTotalOrderCurrentMonth()
        {
            int total = 0;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get current date
                    DateTime now = DateTime.Now;
                    //Get the start date of the current month
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    //Get end date of the current month
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    //Get list orders current month
                    List<Order> orders = context.Orders.Where(o => o.OrderedDate >= startDate).Where(o => o.OrderedDate <= endDate).ToList();
                    //Count number of orders
                    orders.Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return total;
        }
    }
}
