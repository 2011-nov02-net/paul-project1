using Microsoft.EntityFrameworkCore;
using StoreWebApp.EfModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repositories.EfModel;

namespace Repositories
{
    public class CustomerRepo:ICustomerRepo
    {
        private readonly DbContextOptions<project0Context> _contextOptions;
        public CustomerRepo(DbContextOptions<project0Context> contextOptions)
        {
            _contextOptions = contextOptions;
        }
        //Add Customer
        public void AddCustomer(string firstName, string lastName)
        {
            using var context = new project0Context(_contextOptions);
            Customer customer = new Customer()
            {
                FirstName = firstName,
                LastName = lastName
            };
            context.Add(customer);
            context.SaveChanges();
        }
        //Get All Customers
        public List<Customer> GetAllCustomer()
        {
            using var context = new project0Context(_contextOptions);
            var dbCustomers = context.Customers.ToList();
            var result = new List<Customer>();
            foreach (var customer in dbCustomers)
            {
                var _customer = new Customer()
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName
                };
                result.Add(_customer);
            }
            return result;

        }
        //Get Customer By Name
        public Customer GetCustomerByName(string firstName, string lastName)
        {
            using var context = new project0Context(_contextOptions);
            var dbCustomer = context.Customers.FirstOrDefault(c => c.FirstName == firstName && c.LastName == lastName);
            return new Customer(dbCustomer.CustomerId, dbCustomer.FirstName, dbCustomer.LastName);
        }
        //Get Customer By Id
        public Customer GetCustomerById(int customerId)
        {
            using var context = new project0Context(_contextOptions);
            var dbCustomer = context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            return new Customer(dbCustomer.CustomerId, dbCustomer.FirstName, dbCustomer.LastName);
        }
        //Update Customer
        public void UpdateCustomer(Customer customer)
        {
            using var context = new project0Context(_contextOptions);
            var dbCustomer = context.Customers
                .Where(c => c.CustomerId == customer.CustomerId)
                .FirstOrDefault();
            dbCustomer.FirstName = customer.FirstName;
            dbCustomer.LastName = customer.LastName;
            context.SaveChanges();
        }
    }
}
