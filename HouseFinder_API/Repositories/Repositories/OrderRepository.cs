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
        public int[] GetSolvedlOrderByMonth() => OrderDAO.GetSolvedOrderByMonth();
        public int[] GetTotalOrderByMonth() => OrderDAO.GetTotalOrderByMonth();
    }
}
