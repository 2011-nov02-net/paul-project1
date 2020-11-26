using System;
using System.Collections.Generic;

#nullable disable

namespace StoreWebApp.EfModel
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public Order(int orderId, int customerId, int storeId, decimal orderTotalPrice, DateTime date)
        {
            OrderId = orderId;
            CustomerId = customerId;
            StoreId = storeId;
            OrderTotalPrice = orderTotalPrice;
            Date = date;
        }

        public Order(int orderId, int customerId, int storeId, decimal orderTotalPrice, ICollection<OrderItem> orderItems, DateTime date)
        {
            OrderId = orderId;
            CustomerId = customerId;
            StoreId = storeId;
            OrderTotalPrice = orderTotalPrice;
            OrderItems = orderItems;
            Date = date;
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int StoreId { get; set; }
        public decimal OrderTotalPrice { get; set; }
        public DateTime Date { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
