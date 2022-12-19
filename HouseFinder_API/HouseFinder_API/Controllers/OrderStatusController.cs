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
    public class OrderStatusController : ControllerBase
    {
        private IOrderStatusRepository statusRepository = new OrderStatusRepository();

        [HttpGet]
        public IActionResult GetAllOrderStatus()
        {
            List<OrderStatusDTO> status = statusRepository.GetAllOrderStatus();
            //if (status == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
                return Ok(status);
            //}
        }
    }
}
