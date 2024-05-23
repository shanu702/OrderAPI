using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Order.Entities;
using OrderAPI.Services;
using System.Linq;

namespace OrderAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderQueueService _orderQueueService;
        private readonly OrderProcessingService _orderProcessingService;
        private readonly ILogger<OrderController> _logger;
        private readonly IValidator<OrderRequest> _validator;

        public OrderController(OrderQueueService orderQueueService, OrderProcessingService orderProcessingService, ILogger<OrderController> logger, IValidator<OrderRequest> validator)
        {
            _orderQueueService = orderQueueService;
            _orderProcessingService = orderProcessingService;
            _logger = logger;
            _validator = validator; ;
        }

        [HttpPost("place")]
        public IActionResult PlaceOrder([FromBody] OrderRequest orderRequest)
        {
            ValidationResult result = _validator.Validate(orderRequest);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));
            }
            var order = new Order.Entities.Order
            {
                OrderId = orderRequest.OrderId,
                ProductName = orderRequest.ProductName,
                Quantity = orderRequest.Quantity
            };
            _orderQueueService.EnqueueOrder(order);
            _logger.LogInformation("Order placed: {@Order}", order);
            return CreatedAtAction(nameof(PlaceOrder), new { id = order.OrderId }, order);
        }

        [HttpGet]
        public IActionResult GetOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var orders = _orderProcessingService.GetAllOrders(pageNumber, pageSize);
            _logger.LogInformation("Retrieved orders: Page {PageNumber}, Size {PageSize}", pageNumber, pageSize);

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = _orderProcessingService.GetOrderById(id);
            if (order == null)
            {
                _logger.LogWarning("Order not found: {OrderId}", id);
                return NotFound();
            }
            _logger.LogInformation("Retrieved order: {@Order}", order);
            return Ok(order);
        }
    }
}
