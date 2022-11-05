using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObjects;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDAO
    {
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
        //[Staff/Dashboard] Get list of order
        public static List<OrderDTO> GetAllOrders()
        {
            List<OrderDTO> orders;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    orders = context.Orders.ProjectTo<OrderDTO>(config).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return orders;
        }

        //[Staff/list-order] Count total order 
        public static int CountTotalOrder()
        {
            int total;
            try
            {
                total = GetAllOrders().Count();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return total;
        }

        public static int[] GetTotalOrderByMonth()
        {
            int[] totals = new int[12];
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get first date of cureent year
                    DateTime firstDate = new DateTime(DateTime.Now.Year, 1, 1);
                    //Get current date
                    DateTime currentDate = DateTime.Today;

                    //Get list order from the first date to current date
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    List<OrderDTO> orders = context.Orders.Where(o => o.OrderedDate >= firstDate && o.OrderedDate <= currentDate).ProjectTo<OrderDTO>(config).ToList();

                    //Get months' name
                    string[] monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames;

                    //Count total orders by month
                    for (int i = 0; i < 12; i++)
                    {
                        foreach (OrderDTO o in orders)
                        {
                            if (o.OrderedDate.ToString("MMMM dd").Contains(monthNames[i]))
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

        //Calculate number of solved order by month
        public static int[] GetSolvedOrderByMonth()
        {
            int[] totals = new int[12];
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get first date of cureent year
                    DateTime firstDate = new DateTime(DateTime.Now.Year, 1, 1);
                    //Get current date
                    DateTime currentDate = DateTime.Today;

                    //Get list orders solved from the first date to current date
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    List<OrderDTO> orders = context.Orders.Where(o => o.SolvedDate >= firstDate && o.SolvedDate <= currentDate).ProjectTo<OrderDTO>(config).ToList();

                    //Get months' name
                    string[] monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames;

                    //Count total orders by month
                    for (int i = 0; i < 12; i++)
                    {
                        foreach (OrderDTO o in orders)
                        {
                            string dateName = o.SolvedDate != null ? o.SolvedDate.Value.ToString("MMMM dd") : "n/a";
                            if (dateName.Contains(monthNames[i]))
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

        public int OrderByMonth(int month)
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
    }
}
