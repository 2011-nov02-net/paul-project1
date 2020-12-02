using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Models.ViewModel
{
    public class ProductViewModel
    {
        [Required]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        [StringLength(50, ErrorMessage = "Should be the same with Product ID")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        [Range(0, 1000000, ErrorMessage = "No below zero price. Try Again!")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
