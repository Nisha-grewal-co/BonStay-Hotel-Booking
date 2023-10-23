using System;
using System.Collections.Generic;

namespace OnlineFoodOrderDALCrossPlatform.Models
{
    public partial class Item
    {
        public Item()
        {
            Orders = new HashSet<Order>();
        }

        public string ItemId { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
