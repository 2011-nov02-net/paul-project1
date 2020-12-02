using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EfModel.EfModel
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
