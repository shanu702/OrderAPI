using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
