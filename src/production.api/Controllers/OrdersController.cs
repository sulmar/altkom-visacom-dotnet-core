using Microsoft.AspNetCore.Mvc;
using production.models;

namespace production.api.Controllers
{
    [Route("api/[controller]")]
    [Route("api/zamowienia")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {   
            var orders = orderRepository.Get();
            return Ok(orders);
        }
    }
}