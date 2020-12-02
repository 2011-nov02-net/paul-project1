using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Models.ViewModel
{
    public class StoreViewModel
    {
        [Required]
        [Display(Name = "Store ID")]
        public int StoreId { get; set; }

        [Required]
        [Display(Name = "Store Name")]
        [StringLength(50, ErrorMessage = "Too Long, Try Again!")]
        public string StoreName { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
