using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EfModel.Models
{
    public partial class OrderItem
    {
        [Display(Name = "Item ID")]
        [Required]
        public int ItemId { get; set; }

        [Required]
        [Display(Name = "Order ID")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }


        [Required]
        [Display(Name = "Quantity")]
        [Range(1, 1000, ErrorMessage = "Out of Range, Try Again!")]
        public int Quantity { get; set; }

        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        public decimal? Total { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
