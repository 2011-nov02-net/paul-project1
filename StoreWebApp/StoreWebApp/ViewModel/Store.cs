using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace StoreWebApp.ViewModel
{
    public partial class Store
    {
        public Store()
        {
            Inventories = new HashSet<Inventory>();
            Orders = new HashSet<Order>();
        }
        [Required]
        [Display(Name ="Store ID")]
        public int StoreId { get; set; }

        [Required]
        [Display(Name ="Store Name")]
        [StringLength(50, ErrorMessage ="Too Long, Try Again!")]
        public string StoreName { get; set; }

        [Required]
        [Display(Name ="Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
