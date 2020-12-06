using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EfModel.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        [Required]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Too long, Try Again")]
        [RegularExpression("[A-Z][a-z]*$", ErrorMessage = "Starts with Capital Letter A-Z, Try Again!")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Too long, Try Again")]
        [RegularExpression("[A-Z][a-z]*$", ErrorMessage = "Starts with Capital Letter A-Z, Try Again!")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
