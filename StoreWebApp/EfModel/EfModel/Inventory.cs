using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EfModel.EfModel
{
    public partial class Inventory
    {
        public string StoreName { get; set; }
        public string ProductName { get; set; }

        public Inventory()
        {
        }

        public int InventoryId { get; set; }

        public int? StoreId { get; set; }

        public int? ProductId { get; set; }

        public int Stock { get; set; }

        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
    }
}
