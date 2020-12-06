using EfModel.Interfaces;
using EfModel.Models;
using Microsoft.EntityFrameworkCore;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Customer = StoreLibrary.Customer;

namespace EfModel.Repositories
{
    public class CustomerRepo : ICustomer
    {
        private readonly DbContextOptions<project0Context> _context;
        public CustomerRepo(DbContextOptions<project0Context> context)
        {
            _context = context;
        }
        public void CreateCustomer(Customer customer)
        {
            using var context = new project0Context(_context);
            var newEntry = new Models.Customer()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                
            };
            context.Customers.Add(newEntry);
            context.SaveChanges();
        }

        public void DeleteCustomer(Customer customer)
        {
            using var context = new project0Context(_context);
            var dbCustomer = context.Customers
                .Where(i => i.CustomerId == customer.CustomerId)
                .FirstOrDefault();
            context.Remove(dbCustomer);
            context.SaveChanges();
        }

        public List<Customer> GetAllCustomers()
        {
            using var context = new project0Context(_context);
            var dbCust = context.Customers.Distinct().ToList();
            var result = new List<Customer>();
            foreach (var customer in dbCust)
            {
                var newCustomer = new Customer()
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName
                };
                result.Add(newCustomer);
            };
            return result;
        }

        public Customer GetCustomerById(int? id)
        {
            using var context = new project0Context(_context);
            var dbCust = context.Customers
                .Where(c => c.CustomerId == id)
                .FirstOrDefault();
            if (dbCust == null)
            {
                return null;
            }
            var newCust = new Customer()
            {
                CustomerId = dbCust.CustomerId,
                LastName = dbCust.LastName,
                FirstName = dbCust.FirstName
            };
            return newCust;
        }

        public List<Customer> GetCustomerByName(string first, string last)
        {
            using var context = new project0Context(_context);
            var dbCust = context.Customers
                .Where(c => c.FirstName == first)
                .Where(c => c.LastName == last)
                .ToList();
            var allCust = new List<Customer>();
            foreach (var cust in dbCust)
            {
                allCust.Add(GetCustomerById(cust.CustomerId));
            }
            return allCust;
        }

        public void UpdateCustomer(Customer customer)
        {
            using var context = new project0Context(_context);
            var dbCust = context.Customers
                .Where(a => a.CustomerId == customer.CustomerId)
                .FirstOrDefault();
            dbCust.FirstName = customer.FirstName;
            dbCust.LastName = customer.LastName;
            try
            {
                context.SaveChanges();
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
        }  
    }
}
