﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    public class Customer
    {
        public Customer()
        {

        }

        public Customer(int customerId, string firstName, string lastName)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }
        public virtual Order Order { get; set; }
    }
}
