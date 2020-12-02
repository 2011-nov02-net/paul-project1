using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string StoreName { get; set; }
        public int? StoreId { get; set; }
        public int? ProductId { get; set; }
        public int Stock { get; set; }

        public Inventory(int storeId, string productName, int quantity)
        {
            StoreId = storeId;
            ProductName = productName;
            Quantity = quantity;
        }

        public Inventory(string storeName, int? storeId, int? productId, string productName, int stock)
        {
            StoreName = storeName;
            StoreId = storeId;
            ProductId = productId;
            ProductName = productName;
            Stock = stock;
        }

        public Inventory()
        {
        }
    }
}
