using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }


        public int OrderId { get; set; }

        public decimal OrderTotalPrice { get; set; }
        public DateTime Date { get; set; }

        public  Customer Customer { get; set; }
        public  Store Store { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
