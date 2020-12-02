using System;
using System.Collections.Generic;

#nullable disable

namespace StoreWebApp.EfModel
{
    public partial class OrderItem
    {
        public OrderItem()
        {
        }

        public OrderItem(int orderId, Product product, int quantity, decimal purchasePrice)
        {
            OrderId = orderId;
            Product = product;
            Quantity = quantity;
            PurchasePrice = purchasePrice;
        }

        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal PurchasePrice { get; set; }
        public int Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        
        public List<Product> Products { get; set; }
    }
}
