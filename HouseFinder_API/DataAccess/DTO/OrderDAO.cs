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
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(month); ;
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
    }
}
