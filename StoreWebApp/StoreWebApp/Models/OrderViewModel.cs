using StoreLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Models
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            OrderItems = new List<OrderItemViewModel>();
            StoreList = new List<Store>();
            CustomerList = new List<Customer>();
            Products = new List<Product>();
        }
        public OrderViewModel(Order order)
        {
            OrderId = order.OrderId;
            Store = order.Store.StoreId;
            StoreName = order.Store.StoreName;
            Customer = order.Customer.CustomerId;
            CustomerName = order.Customer.FirstName;
            Total = order.OrderTotalPrice;
            Date = order.Date;
            StoreList = new List<Store>();
            CustomerList = new List<Customer>();
            Products = new List<Product>();
            foreach (var orderItem in order.OrderItems)
            {
                var newOrderItem = new OrderItemViewModel(orderItem);
                OrderItems.Add(newOrderItem);
            };
        }
        
        [Display(Name = "Order ID")]
        public int OrderId { get; set; }
        [Required]
        [Display(Name = "Store ID")]
        public int Store{ get; set; }
        [Display(Name = "Store Name")]
        public string StoreName { get; set; }
        [Required]
        public int Customer { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public List<Store> StoreList { get; set; }
        public List<Customer> CustomerList { get; set; }
        public List<Product> Products { get; set; }
    }
}
