using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StoreLibrary;

#nullable disable

namespace EfModel.EfModel
{
    public partial class Product
    {
        public Product()
        {
            Inventories = new HashSet<Inventory>();
            OrderItems = new HashSet<OrderItem>();
        }
        public int ProductId { get; set; }
        
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
