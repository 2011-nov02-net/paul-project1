using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    public class Order
    {
        public int StoreId { get; set; }
        public int OrderId { get; set; }
        public Customer Customer { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Order(int storeId, Customer customer, decimal total)
        {
            StoreId = storeId;
            Customer = customer;
            Total = total;
            OrderItems = new List<OrderItem>();
        }
    }
}
