using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
namespace OrderAPI.Services
{
    public class OrderProcessingService
    {
        private readonly ConcurrentBag<Order.Entities.Order> _inMemoryStorage = new ConcurrentBag<Order.Entities.Order>();

        public void AddOrder(Order.Entities.Order order)
        {
            _inMemoryStorage.Add(order);
        }

        public IEnumerable<Order.Entities.Order> GetAllOrders(int pageNumber, int pageSize)
        {
            return _inMemoryStorage.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public Order.Entities.Order GetOrderById(int orderId)
        {
            return _inMemoryStorage.FirstOrDefault(o => o.OrderId == orderId);
        }
    }
}
