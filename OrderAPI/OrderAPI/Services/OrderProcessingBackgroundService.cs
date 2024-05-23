using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace OrderAPI.Services
{
    public class OrderProcessingBackgroundService: BackgroundService
    {
        private readonly OrderQueueService _orderQueueService;
        private readonly OrderProcessingService _orderProcessingService;
        private readonly ILogger<OrderProcessingBackgroundService> _logger;

        public OrderProcessingBackgroundService(
            OrderQueueService orderQueueService,
            OrderProcessingService orderProcessingService,
            ILogger<OrderProcessingBackgroundService> logger)
        {
            _orderQueueService = orderQueueService;
            _orderProcessingService = orderProcessingService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_orderQueueService.TryDequeueOrder(out var order))
                {
                    _orderProcessingService.AddOrder(order);
                    _logger.LogInformation($"Order {order.OrderId} processed.");
                }
                await Task.Delay(1000, stoppingToken); // Simulate processing time
            }
        }
    }
}
