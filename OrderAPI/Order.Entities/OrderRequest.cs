using System;
using System.ComponentModel.DataAnnotations;

namespace Order.Entities
{
    public class OrderRequest
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
