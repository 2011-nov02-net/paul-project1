using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    public class Store
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public List<Inventory> Inventory { get; set; }
        public Store()
        {
            Inventory = new List<Inventory>();
        }

    }
}
