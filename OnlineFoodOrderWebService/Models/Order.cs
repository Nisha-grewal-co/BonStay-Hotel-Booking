using OnlineFoodOrderDALCrossPlatform.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodOrderWebService.Models
{
    public class Order
    {
        public Customer Customer { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        public string DeliveryStatus { get; set; }

        public Item Item { get; set; }

        [Required]
        public string ItemId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public int OrderId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }
    }
}
