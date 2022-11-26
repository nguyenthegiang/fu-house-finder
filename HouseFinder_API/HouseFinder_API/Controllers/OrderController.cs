using BusinessObjects;
using DataAccess.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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
        private IUserReposiotry userReposiotry = new UserRepository();

        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<OrderDTO>> GetAllOrders() => orderRepository.GetAllOrder();

        //GET: api/Order/CountTotalOrder
        [HttpGet("CountTotalOrder")]
        public int CountTotalOrder()
        {
            int total = orderRepository.CountTotalOrder();
            return total;
        }

        //GET: api/Order/GetTotalOrderByMonth
        [HttpGet("CountTotalOrderOrderedInMonth")]
        public IActionResult CountTotalOrderOrderedInMonth()
        {
            int[] totals = orderRepository.CountTotalOrderOrderedInMonth();
            return Ok(totals);
        }
        //GET: api/Order/GetSolvedOrderByMonth
        [HttpGet("CountTotalOrderSolvedInMonth")]
        public IActionResult CountTotalOrderSolvedInMonth()
        {
            int[] totals = orderRepository.CountTotalOrderSolvedInMonth();
            return Ok(totals);
        }

        //Update reservation
        [Authorize]
        [HttpPut("{orderId}/{statusId}")]
        public IActionResult UpdateReservation(int orderId, int statusId)
        {
            try
            {
                string uid = HttpContext.Session.GetString("User");
                if (uid == null)
                {
                    return Forbid();
                }
                orderRepository.UpdateOrderStatus(orderId, statusId, uid);
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        //Count total order
        [Authorize]
        [HttpGet("CountTotalOrderSolvedBy")]
        public IActionResult CountTotalOrderSolvedBy()
        {
            string uid = HttpContext.Session.GetString("User");
            if(uid == null)
            {
                return Forbid();
            }
            return Ok(orderRepository.CountTotalOrderSolvedByAccount(uid));
        }

        [Authorize]
        [HttpGet("CountSolvedOrderByStaffInAYear")]
        public IActionResult CountSolvedOrderByStaffInAYear()
        {
            string uid = HttpContext.Session.GetString("User");
            if (uid == null)
            {
                return Forbid();
            }
            return Ok(orderRepository.CountSolvedOrderByStaffInAYear(uid));
        }

        //[User] Add Order
        [HttpPost]
        public IActionResult Post(Order order)
        {
            try
            {
                string uid = HttpContext.Session.GetString("User");
                if (uid == null)
                {
                    return Forbid();
                }
                UserDTO user = userReposiotry.GetUserByID(uid);
                //Set default status
                order.StatusId = 1;
                //Set default date
                order.OrderedDate = DateTime.Now;
                //add user order
                order.StudentId = user.UserId;
                //Add to DB
                orderRepository.AddOrder(order);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("CountTotalOrderByMonth")]
        public IActionResult CountTotalOrderByMonth()
        {
            int[] totals = orderRepository.CountTotalOrderByMonth();
            return Ok(totals);
        }

        [HttpGet("CountSolvedOrderByMonth")]
        public IActionResult CountSolvedOrderByMonth()
        {
            int[] totals = orderRepository.CountSolvedOrderByMonth();
            return Ok(totals);
        }

        [Authorize]
        [HttpGet("CountSolvedOrderByStaffInDate/{date}")]
        public IActionResult CountSolvedOrderByStaffInDate(DateTime date)
        {
            string uid = HttpContext.Session.GetString("User");
            if (uid == null)
            {
                return Forbid();
            }
            return Ok(orderRepository.CountSolvedOrderByStaffInDate(date, uid));
        }
    }
}
