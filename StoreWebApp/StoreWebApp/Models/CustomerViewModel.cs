using StoreLibrary;
using System.ComponentModel.DataAnnotations;

namespace StoreWebApp.Models
{
    public class CustomerViewModel
    {
        public CustomerViewModel(Customer customer)
        {
            CustomerId = customer.CustomerId;
            LastName = customer.LastName;
            FirstName = customer.FirstName;
        }
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Too long, Try Again")]
        [RegularExpression("[A-Z][a-z]*$", ErrorMessage = "Starts with Capital Letter A-Z, Try Again!")]
        public string LastName { get; set; }
        
        [Required]
        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Too long, Try Again")]
        [RegularExpression("[A-Z][a-z]*$", ErrorMessage = "Starts with Capital Letter A-Z, Try Again!")]
        public string FirstName { get; set; }
    }
}
