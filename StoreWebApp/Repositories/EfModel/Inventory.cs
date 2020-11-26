using System;
using System.Collections.Generic;

#nullable disable

namespace StoreWebApp.EfModel
{
    public partial class Inventory
    {
        public Inventory()
        {
        }

        public Inventory(string storeName, int? storeId, int? productId, string productName, int stock)
        {
            StoreName = storeName;
            StoreId = storeId;
            ProductId = productId;
            ProductName = productName;
            Stock = stock;
        }

        public int InventoryId { get; set; }
        public int? StoreId { get; set; }
        public int? ProductId { get; set; }
        public int Stock { get; set; }
        public string StoreName { get; set; }
        public string ProductName { get; set; }

        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
    }
}
