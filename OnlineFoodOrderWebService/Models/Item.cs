using OnlineFoodOrderDALCrossPlatform.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodOrderWebService.Models
{
    public class Item
    {
        public Category Category { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string ItemId { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public ICollection<Order> Orders { get; set; }

        [Required]
        public decimal Price { get; set; }

        public Item()
        {
            Orders = new HashSet<Order>();
        }
    }
}
