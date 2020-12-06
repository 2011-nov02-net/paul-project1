using StoreLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Models
{
    public class InventoryViewModel
    {
        public InventoryViewModel()
        {
            Products = new List<Product>();
        }
        public InventoryViewModel(Inventory inventory)
        {
            InventoryId = inventory.InventoryId;
            StoreId = inventory.StoreId;
            ProductId = inventory.ProductId;
            ProductName = inventory.ProductName;
            Stock = inventory.Stock;
            Products = new List<Product>();
        }
        [Required]
        [Display(Name = "Inventory ID")]
        public int InventoryId { get; set; }
        
        [Required]
        [Display(Name = "Store ID")]
        public int StoreId { get; set; }

        [Required]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        
        [Required]
        [Display(Name = "Stock")]
        [Range(0, 1000000, ErrorMessage = "No negative stock, Try Again")]
        public int Stock { get; set; }
        public List<Product> Products { get; set; }
    }
}
