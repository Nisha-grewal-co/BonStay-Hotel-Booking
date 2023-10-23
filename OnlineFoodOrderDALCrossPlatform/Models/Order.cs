using System;
using System.Collections.Generic;

namespace OnlineFoodOrderDALCrossPlatform.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string ItemId { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string DeliveryAddress { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string DeliveryStatus { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;
        public virtual Item Item { get; set; } = null!;
    }
}
