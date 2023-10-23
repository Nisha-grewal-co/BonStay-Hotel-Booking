using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineFoodOrderDALCrossPlatform.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public string DeliveryAddress { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string DeliveryStatus { get; set; } = null!;
    }
}
