using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EfModel.EfModel
{
    public partial class Customer
    {
        [Required]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Too long, Try Again")]
        [RegularExpression("[A-Z]*$", ErrorMessage = "Starts with Capital Letter A-Z, Try Again!")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Too long, Try Again")]
        [RegularExpression("[A-Z]*$", ErrorMessage = "Starts with Capital Letter A-Z, Try Again!")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual Order Order { get; set; }
    }
}
