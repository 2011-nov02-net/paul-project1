using StoreLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Models
{
    public class OrderItemViewModel
    {
        public OrderItemViewModel()
        {
            Products = new List<Product>();
        }
        public OrderItemViewModel(OrderItem orderItem)
        {
            OrderId = orderItem.OrderId;
            ProductId = orderItem.Product.ProductId;
            Quantity = orderItem.Quantity;
            Total = orderItem.PurchasePrice;
            Products = new List<Product>();
        }
        
        [Display(Name = "Item ID")]
        public int ItemId { get; set; }
        
        [Required]
        [Display(Name = "Order ID")]
        public int OrderId { get; set; }
        
        [Required]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }
        
        [Required]
        [Display(Name = "Quantity")]
        [Range(1, 1000, ErrorMessage = "Out of Range, Try Again!")]
        public int Quantity { get; set; }
        
        [Required]
        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        public List<Product> Products { get; set; }
    }
}
