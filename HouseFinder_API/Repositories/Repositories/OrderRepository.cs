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
    public class OrderRepository : IOrderRepository
    {
        public int CountTotalOrder() => OrderDAO.CountTotalOrder();
        public List<OrderDTO> GetAllOrder() => OrderDAO.GetAllOrders();
        public int[] CountTotalOrderSolvedInMonth() => OrderDAO.CountTotalOrderSolvedInMonth();
        public int[] CountTotalOrderOrderedInMonth() => OrderDAO.CountTotalOrderOrderedInMonth();

        public void UpdateOrderStatus(int orderId, int statusId, string account) => OrderDAO.UpdateOrderStatus(orderId, statusId, account);

        public void AddOrder(Order order) => OrderDAO.AddOrder(order);

        public int CountTotalOrderSolvedByAccount(string account) => OrderDAO.CountTotalOrderSolvedByAccount(account);

        public int[] CountSolvedOrderByStaffInAYear(string account) => OrderDAO.CountSolvedOrderByStaffInAYear(account);

        public int[] CountTotalOrderByMonth() => OrderDAO.CountTotalOrderByMonth();

        public int[] CountSolvedOrderByMonth() => OrderDAO.CountSolvedOrderByMonth();

        public int CountSolvedOrderByStaffInDate(DateTime date, string account) => OrderDAO.CountSolvedOrderByStaffInDate(date, account);
    }
}
