using System;
using System.Collections.Generic;

#nullable disable

namespace EfModel.Models
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }

        public virtual Order Order { get; set; }
    }
}
