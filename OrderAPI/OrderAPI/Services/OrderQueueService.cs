using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderAPI.Services
{
    public class OrderQueueService
    {
        private readonly ConcurrentQueue<Order.Entities.Order> _orderQueue = new ConcurrentQueue<Order.Entities.Order>();

        public void EnqueueOrder(Order.Entities.Order order)
        {
            _orderQueue.Enqueue(order);
        }

        public bool TryDequeueOrder(out Order.Entities.Order order)
        {
            return _orderQueue.TryDequeue(out order);
        }
    }
}
