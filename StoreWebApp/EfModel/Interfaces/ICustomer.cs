using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfModel.Interfaces
{
    public interface ICustomer
    {
        List<Customer> GetAllCustomers();
        List<Customer> GetCustomerByName(string first, string last);
        Customer GetCustomerById(int? id);
        void CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
    }
}
