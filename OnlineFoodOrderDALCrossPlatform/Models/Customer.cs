using System;
using System.Collections.Generic;

namespace OnlineFoodOrderDALCrossPlatform.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string EmailId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
