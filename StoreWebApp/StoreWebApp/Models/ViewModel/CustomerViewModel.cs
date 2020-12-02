using StoreLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Models.ViewModel
{
    public class CustomerViewModel
    {

        [Required]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Too long, Try Again")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Too long, Try Again")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual Order Order { get; set; }
    }
}
