using System;
using System.Collections.Generic;

#nullable disable

namespace EfModel.Models
{
    public partial class Inventory
    {
        public int InventoryId { get; set; }
        public int? StoreId { get; set; }
        public int? ProductId { get; set; }
        public int Stock { get; set; }

        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
    }
}
