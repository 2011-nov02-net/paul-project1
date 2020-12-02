
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfModel.Interfaces
{
    public interface ICustomerRepo
    {
        public void AddCustomer(string firstName, string lastName);
        public List<Customer> GetAllCustomers();
        public Customer GetCustomerById(int customerId);
        public List<Customer> GetCustomerByName(string firstName, string lastName);

    }
}
