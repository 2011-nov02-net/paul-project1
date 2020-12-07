using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EfModel.Models
{
    public partial class Inventory
    {
        [Display(Name = "Inventory ID")]
        public int InventoryId { get; set; }

        [Display(Name = "Store ID")]
        public int StoreId { get; set; }

        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Stock")]
        [Range(0, 1000000, ErrorMessage = "No negative stock, Try Again")]
        public int Stock { get; set; }

        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
    }
}
