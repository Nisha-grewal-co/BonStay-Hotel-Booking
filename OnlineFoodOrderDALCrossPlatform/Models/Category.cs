using System;
using System.Collections.Generic;

namespace OnlineFoodOrderDALCrossPlatform.Models
{
    public partial class Category
    {
        public Category()
        {
            Items = new HashSet<Item>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Item> Items { get; set; }
    }
}
