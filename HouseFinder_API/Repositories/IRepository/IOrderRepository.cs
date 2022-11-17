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
        public int[] GetTotalOrderByMonth();
        public int[] GetSolvedlOrderByMonth();
        public void UpdateOrderStatus(int orderId, int statusId);
        public void AddOrder(Order order);

    }
}
