using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EfModel.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }
        [Required]
        [Display(Name = "Order ID")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Store ID")]
        public int StoreId { get; set; }


        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        public decimal OrderTotal { get; set; }
        public DateTime Date { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
