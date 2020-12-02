using EfModel.EfModel;
using EfModel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoreLibrary;

namespace EfModel.Repositories
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly DbContextOptions<project0Context> _contextOptions;
        public CustomerRepo(DbContextOptions<project0Context> contextOptions)
        {
            _contextOptions = contextOptions;
        }
        //Add Customers
        public void AddCustomer(string firstName, string lastName)
        {
            using var context = new project0Context(_contextOptions);
            StoreLibrary.Customer customer = new StoreLibrary.Customer()
            {
                FirstName = firstName,
                LastName = lastName
            };
            context.Add(customer);
            context.SaveChanges();
        }
        //List All Customers
        public List<StoreLibrary.Customer> GetAllCustomers()
        {
            using var context = new project0Context(_contextOptions);
            var dbCustomers = context.Customers.ToList();
            var result = new List<StoreLibrary.Customer>();
            foreach (var customer in dbCustomers)
            {
                var _customer = new StoreLibrary.Customer()
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName
                };
                result.Add(_customer);
            }
            return result;
        }

        public StoreLibrary.Customer GetCustomerById(int customerId)
        {
            using var context = new project0Context(_contextOptions);
            var dbCustomer = context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            return new StoreLibrary.Customer(dbCustomer.CustomerId, dbCustomer.FirstName, dbCustomer.LastName);
        }

        public List<StoreLibrary.Customer> GetCustomerByName(string firstName, string lastName)
        {
            using var context = new project0Context(_contextOptions);
            var dbCustomer = context.Customers
                .Where(c => c.FirstName == firstName)
                .Where(c => c.LastName == lastName)
                .ToList();
            var allCustomer = new List<StoreLibrary.Customer>();
            foreach (var customer in dbCustomer)
            {
                allCustomer.Add(GetCustomerById(customer.CustomerId));
            }
            return allCustomer;
        }
    }
}
