using AutoMapper;
using BusinessObjects;
using System;
using System.Collections.Generic;
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
                    //string monthName = "";
                    ////Get list orders by month
                    ////List<Order> orders = context.Orders.Where(o => o.OrderedDate.ToString("MMMM dd").Contains(monthName)).ToList();
                    //List<Order> orders = context.Orders.ToList();
                    //foreach (Order o in orders)
                    //{
                    //    if( o.OrderedDate != null)
                    //    {
                    //        String tday = DateTime.Now.ToString("MM/dd/yyyy");
                    //    }
                    //}
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
