using BusinessObjects;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IOrderRepository
    {
        public List<OrderDTO> GetAllOrder();
        public int CountTotalOrder();
        public int[] CountTotalOrderOrderedInMonth();
        public int[] CountTotalOrderSolvedInMonth();
        public void UpdateOrderStatus(int orderId, int statusId, string account);
        public void AddOrder(Order order);
        public int CountTotalOrderSolvedByAccount(string account);
        public int[] CountSolvedOrderByStaffInAYear(string account);
        public int[] CountTotalOrderByMonth();
        public int[] CountSolvedOrderByMonth();
        public int CountOrderSolvedByStaffInADay(DateTime date, string account);
        public List<Order> getListOrderNotConfirm(string uId);

    }
}
