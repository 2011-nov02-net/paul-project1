using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    public class OrderItem
    {

        public OrderItem(int orderId, Product product, int quantity, decimal purchasePrice)
        {
            OrderId = orderId;
            Product = product;
            Quantity = quantity;
            PurchasePrice = purchasePrice;
        }

        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public decimal PurchasePrice { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
    }
}
