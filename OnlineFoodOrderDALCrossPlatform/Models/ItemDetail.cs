using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineFoodOrderDALCrossPlatform.Models
{
    public class ItemDetail
    {
        [Key]
        public string ItemId { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
