using System;
using System.Collections.Generic;

#nullable disable

namespace StoreWebApp.ViewModel
{
    public partial class OrderItem
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal PurchasePrice { get; set; }
        public int Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public List<Product> Products { get; set; }
    }
}
