using EfModel.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreLibrary;
using StoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ICustomer _customerRepo;
        private readonly IStore _storeRepo;
        private readonly IOrder _orderRepo;
        private readonly IProduct _productRepo;
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(ILogger<OrdersController> logger, ICustomer customerRepo, IStore storeRepo, IOrder orderRepo, IProduct productRepo)
        {
            _logger = logger;
            _customerRepo = customerRepo;
            _storeRepo = storeRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
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
                throw new Exception("Controller Error!");
            }
            else
            {
                var result = _orderRepo.GetOrderById(id);
                if (result == null)
                {
                    throw new Exception("Controller Error!");
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
            var currentLocation = _storeRepo.GetStoreById(orderViewModel.StoreId);
            var currentCustomer = _customerRepo.GetCustomerById(orderViewModel.CustomerId);
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
                var newItem = new OrderItem(0, animal, orderItem.Quantity, orderItem.Total);
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

        public IActionResult Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Controller Error!");
            }
            return View();
        }

        // POST: OrdersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Order order)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Controller Error!");
            }
            var order = _orderRepo.GetOrderById(id);
            if (order == null)
            {
                throw new Exception("Controller Error!");
            }
            return View(order);
        }


        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var order = _orderRepo.GetOrderById(id);
            _orderRepo.DeleteOrder(order);
            return RedirectToAction(nameof(Index));
        }
    }
}
