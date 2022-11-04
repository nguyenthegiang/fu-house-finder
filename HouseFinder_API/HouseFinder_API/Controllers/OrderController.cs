using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFinder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderRepository orderRepository = new OrderRepository();

        [HttpGet]
        public ActionResult<IEnumerable<OrderDTO>> GetAllCampuses() => orderRepository.GetAllOrder();

        //GET: api/Order/CountTotalOrder
        [HttpGet("CountTotalOrder")]
        public int CountTotalOrder()
        {
            int total = orderRepository.CountTotalOrder();
            return total;
        }

    }
}
