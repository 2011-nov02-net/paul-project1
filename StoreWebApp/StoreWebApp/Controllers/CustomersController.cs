using EfModel.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreLibrary;
using StoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomer _customerRepo;
        private readonly IOrder _orderRepo;
        private readonly ILogger<CustomersController> _logger;
        public CustomersController(ILogger<CustomersController> logger, ICustomer customerRepo, IOrder orderRepo)
        {
            _logger = logger;
            _customerRepo = customerRepo;
            _orderRepo = orderRepo;
        }
        // GET: CustomersController
        public IActionResult Index(string firstName)
        {
            List<Customer> customerList = _customerRepo.GetAllCustomers();
            var result = new List<CustomerViewModel>();
            foreach (var cust in customerList)
            {
                var newCust = new CustomerViewModel(cust);
                result.Add(newCust);
            }
            if (!String.IsNullOrEmpty(firstName))
            {
                result = (List<CustomerViewModel>)result.Where(s => s.FirstName.Contains(firstName));
            }
            return View(result);
        }

        // GET: CustomersController/Details/5
        public IActionResult Details(int id)
        {
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
            if (id == null)
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
            {
                throw new Exception("Controller Error");
            }
            var customer = _customerRepo.GetCustomerById(id);
            if (customer == null)
            {
                throw new Exception("Controller Error");
            }
            var result = new CustomerViewModel(customer);
            return View(result);
        }

        // GET: CustomersController/Create
        public IActionResult Create()
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Controller Error");
            }
            return View();
        }

        // POST: CustomersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            try
            {
                _customerRepo.CreateCustomer(customer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomersController/Edit/5
        public IActionResult Edit(int? id)
        {
           // if (id == null)
           // {
           //     throw new Exception("Controller Error");
           // }
            var customer = _customerRepo.GetCustomerById(id);
           //if (customer == null)
           // {
           //     throw new Exception("Controller Error");
           // }
            var result = new CustomerViewModel(customer);
            return View(result);

        }

        // POST: CustomersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                throw new Exception("ID cannot be found");
            }
            if (ModelState.IsValid)
            try
            {
                _customerRepo.UpdateCustomer(customer);
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateConcurrencyException)
            {

                throw new Exception("Db Error!");
                   
            }
            return View(customer);
        }

        // GET: CustomersController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("Controller Error!");
            }
            var customer = _customerRepo.GetCustomerById(id);
            if (customer == null)
            {
                throw new Exception("Customer not Found");
            }
            var result = new CustomerViewModel(customer);
            return View(result);
        }

        // POST: CustomersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                var customer = _customerRepo.GetCustomerById(id);
                _customerRepo.DeleteCustomer(customer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult CustomerOrders(int id)
        {
            var customer = _customerRepo.GetCustomerById(id);
            List<Order> result = _orderRepo.GetOrdersByCustomer(customer);
            return View(result);
        }
    }
}
