using StoreWebApp.EfModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.EfModel
{
    public interface ICustomerRepo
    {
        public void AddCustomer(string firstName, string lastName);
        public List<Customer> GetAllCustomer();
        public Customer GetCustomerById(int customerId);
        public void UpdateCustomer(Customer customer);
    }
}
