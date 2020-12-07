using EfModel.Interfaces;
using EfModel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreLibrary;
using StoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Order = StoreLibrary.Order;
using OrderItem = StoreLibrary.OrderItem;

namespace StoreWebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ICustomer _customerRepo;
        private readonly IStore _storeRepo;
        private readonly IOrder _orderRepo;
        private readonly IProduct _productRepo;
        private readonly ILogger<OrdersController> _logger;
        private readonly project0Context _context;
        public OrdersController(ILogger<OrdersController> logger, ICustomer customerRepo, IStore storeRepo, IOrder orderRepo, IProduct productRepo, project0Context context)
        {
            _logger = logger;
            _customerRepo = customerRepo;
            _storeRepo = storeRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _context = context;
        }


        // GET: OrdersController
        public IActionResult Index(string searchString)
        {
            List<Order> orderList = _orderRepo.GetAllOrders();
            var result = new List<OrderViewModel>();
            foreach (var ord in orderList)
            {
                var newOrd = new OrderViewModel(ord);
                result.Add(newOrd);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.FindAll(s => s.CustomerName.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }
            return View(result);
        }


        // GET: OrdersController/Details/5
        public IActionResult Details(int id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            else
            {
                var result = _orderRepo.GetOrderById(id);
                if(result == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(result);
                }
            }
        }


        // GET: OrdersController/Create
        public IActionResult Create()
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Controller Error!");
            }
            var result = new OrderViewModel();
            var locationList = _storeRepo.GetAllStore();
            var customerList = _customerRepo.GetAllCustomers();
            foreach (var location in locationList)
            {
                result.StoreList.Add(location);
            }
            foreach (var customer in customerList)
            {
                result.CustomerList.Add(customer);
            }
            result.Total = 0;
            return View(result);
        }

        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderViewModel orderViewModel)
        {
           // if (!ModelState.IsValid)
           // {
           //     throw new Exception("Controller Error!");
           // }
            var currentLocation = _storeRepo.GetStoreById(orderViewModel.Store);
            var currentCustomer = _customerRepo.GetCustomerById(orderViewModel.Customer);
            var result = new Order
            {
                Store = currentLocation,
                Customer = currentCustomer,
                Date = DateTime.Now,
                OrderTotalPrice = orderViewModel.Total
            };
            foreach (var orderItem in orderViewModel.OrderItems)
            {
                var animal = _productRepo.GetProductById(orderItem.ProductId);
                var newItem = new StoreLibrary.OrderItem(0, animal, orderItem.Quantity, orderItem.Total);
                result.OrderItems.Add(newItem);
            }
            var resultOrder = _orderRepo.ReturnOrder(result);
            return RedirectToAction("AddOrderItem", new { Id = resultOrder.OrderId});
        }

        public IActionResult AddOrderItem(int id)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Controller Error!");
            }
            var result = new OrderItemViewModel
            {
                OrderId = id,
                Quantity = 1
            };
            var currentOrder = _orderRepo.GetOrderById(id);
            var currentInventory = _storeRepo.GetInventoryByStore(currentOrder.Store);
            var products = _productRepo.GetAllProducts();
            foreach (var product in products)
            {
                if (currentInventory.Any(a => (a.ProductName == product.ProductName && a.Stock > 0))) 
                {
                    result.Products.Add(product);
                }
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrderItem(OrderItemViewModel orderItem)
        {
            if (ModelState.IsValid)
            {
                var order = _orderRepo.GetOrderById(orderItem.OrderId); 
                var product = _productRepo.GetProductById(orderItem.ProductId);
                var locationInventory = _storeRepo.GetInventoryByStore(order.Store);
                var invItem = locationInventory.Find(i => i.ProductName == product.ProductName);
                if (invItem.Stock - orderItem.Quantity < 0) 
                {
                    TempData["QuantityError"] = $"Error. Quantity is too high, not enough {product.ProductName}(s) in inventory. Currently have {invItem.Stock} {product.ProductName}(s) in stock.";
                    return RedirectToAction("AddOrderItem", new { OrderId = orderItem.OrderId });
                }
                else
                {
                    bool existInOrder = order.OrderItems.Any(o => o.Product.ProductId == product.ProductId);
                    var total = product.Price * orderItem.Quantity;
                    if (existInOrder) 
                    {
                        foreach (var thing in order.OrderItems)
                        {
                            if (thing.Product.ProductId == orderItem.ProductId) 
                            {
                                var existingOrder = _orderRepo.GetOrderItemById(thing.ItemId);
                                existingOrder.Quantity += orderItem.Quantity;
                                existingOrder.PurchasePrice += (decimal)total;
                                order.OrderTotalPrice += (decimal)total;
                                _orderRepo.UpdateOrderItem(existingOrder);
                                _orderRepo.UpdateOrder(order);
                            }
                        };
                    }
                    else
                    {
                        var newItem = new OrderItem(orderItem.OrderId, product, orderItem.Quantity, (decimal)total);
                        order.OrderTotalPrice += (decimal)total;
                        _orderRepo.CreateOrderItem(newItem);
                        _orderRepo.UpdateOrder(order);
                    }
                    invItem.Stock -= orderItem.Quantity; 
                    _storeRepo.UpdateInventory(invItem.StoreId, invItem.ProductName, invItem.Stock);
                    TempData["AddOrderItemSuccess"] = $"Success! Added {orderItem.Quantity} {product.ProductName}(s) to your order.";
                    return RedirectToAction("AddOrderItem", new { OrderId = order.OrderId });
                }
            }
            else
            {
                throw new Exception("Controller Error!");
            }
        }
        // GET: Orders1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FirstName", order.CustomerId);
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "StoreName", order.StoreId);
            return View(order);
        }

        // POST: Orders1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,StoreId,OrderTotal,Date")] EfModel.Models.Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FirstName", order.CustomerId);
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "StoreName", order.StoreId);
            return View(order);
        }

        // GET: Orders1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Store)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
