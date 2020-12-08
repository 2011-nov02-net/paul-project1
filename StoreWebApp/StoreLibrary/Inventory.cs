using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public string ProductName { get; set; }
        public int StoreId { get; set; }
        public int Stock { get; set; }

        public Inventory(int storeId, string productName, int quantity)
        {
            StoreId = storeId;
            ProductName = productName;
            Stock = quantity;
        }
        public Inventory()
        {
        }
    }
}
