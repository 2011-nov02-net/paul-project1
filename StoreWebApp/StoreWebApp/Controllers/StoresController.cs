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
    public class StoresController : Controller
    {
        private readonly IStore _storeRepo;
        private readonly IProduct _productRepo;
        private readonly IOrder _orderRepo;
        private readonly ILogger<StoresController> _logger;
        public StoresController(ILogger<StoresController> logger, IStore storeRepo, IProduct productRepo, IOrder orderRepo)
        {
            _logger = logger;
            _storeRepo = storeRepo;
            _productRepo = productRepo;
            _orderRepo = orderRepo;
        }

        // GET: StoresController
        public IActionResult Index()
        {
            var result = _storeRepo.GetAllStore().Select(x => new StoreViewModel
            {
                StoreId = x.StoreId,
                StoreName = x.StoreName
            });
            return View(result);
        }

        // GET: StoresController/Details/5
        public IActionResult Details(int id)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Controller Error!");
            }
            var location = _storeRepo.GetStoreById(id);
            if (location == null)
            {
                throw new Exception("Controller Error!");
            }
            TempData["StoreId"] = location.StoreId;
            return View(location);
        }

        // GET: StoresController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StoresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Store store)
        {
            try
            {
                if (StoreExists(store.StoreName))
                {
                    TempData["StoreExistError"] = $"Store '{store.StoreName}' already exists.";
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    _storeRepo.CreateStore(store);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        //Inventories
        // GET: Location/CreateInventory
        public IActionResult ImportProduct(int id)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Controller Error!");
            }
            var result = new InventoryViewModel();
            var productList = _productRepo.GetAllProducts();
            result.StoreId = id;
            result.Stock = 1;
            foreach (var product in productList)
            {
                var newProduct = new Product()
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price
                };
                result.Products.Add(newProduct);
            }
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportProduct(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                var location = _storeRepo.GetStoreById(inventory.StoreId);
                var productCheck = ProductExists(inventory.ProductId);
                if (productCheck != false) 
                {
                    var product = _productRepo.GetProductById(inventory.ProductId);
                    var existInventory = location.Inventory.Any(i => i.ProductId == inventory.ProductId);
                    if (existInventory)
                    {
                        var invItem = location.Inventory.Find(i => i.ProductName == inventory.ProductName);
                        invItem.Stock += inventory.Stock;
                        _storeRepo.UpdateInventory(location.StoreId, product.ProductName, invItem.Stock);
                        return RedirectToAction("Details", new { id = location.StoreId });
                    }
                    else
                    {
                        _storeRepo.CreateInventory(location, product, inventory.Stock);
                        return RedirectToAction("Details", new { id = location.StoreId });
                    }
                }
            }
            else
            {
                throw new Exception("Controller Error!");
            }
            return View();
        }


        // GET: StoresController/Edit/5
        public IActionResult Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Controller Error!");
            }
            var location = _storeRepo.GetStoreById(id);
            if (location == null)
            {
                throw new Exception("Controller Error");
            }
            var result = new StoreViewModel(location);
            return View(result);
        }

        // POST: StoresController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,City")] Store store)
        {
            if (id != store.StoreId)
            {
                return NotFound();
            }
            if (StoreExists(store.StoreName))
            {
                TempData["StoreExistError"] = $"Store '{store.StoreName}' already exists.";
                return RedirectToAction("Edit", new { id = store.StoreId });
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    _storeRepo.UpdateStore(store);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (StoreExists(id))
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
            return View(store);
        }

        public IActionResult AddToInventory(int id)
        {
            if (id < 0)
            {
                return NotFound();
            }

            var inventoryItem = _storeRepo.GetInventoryById(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }
            var result = new InventoryViewModel(inventoryItem);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToInventory(Inventory inventoryItem)
        {
            if (ModelState.IsValid)
            {
                var location = _storeRepo.GetStoreById(inventoryItem.StoreId);
                var invItem = location.Inventory.Find(i => i.ProductName == inventoryItem.ProductName);
                invItem.Stock += inventoryItem.Stock;
                _storeRepo.UpdateInventory(inventoryItem.StoreId, inventoryItem.ProductName, invItem.Stock);
                return RedirectToAction("Details", new { id = inventoryItem.StoreId });
            }
            return View();
        }

        public IActionResult StoreOrders(int id)
        {
            var location = _storeRepo.GetStoreById(id);
            List<Order> result = _orderRepo.GetOrdersByStore(location);
            return View(result);
        }
        
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Controller Error!");
            }
            var location = _storeRepo.GetStoreById(id);
            if (location == null)
            {
                throw new Exception("Controller Error!");
            }

            return View(location);
        }

        // POST: StoresController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var location = _storeRepo.GetStoreById(id);
            _storeRepo.DeleteStore(location);
            return RedirectToAction(nameof(Index));
        }
        private bool StoreExists(string storeName)
        {
            bool exist = (_storeRepo.GetLocationByName(storeName) != null);
            return exist;
        }
        private bool StoreExists(int id)
        {
            try
            {
                _storeRepo.GetStoreById(id);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool ProductExists(int id)
        {
            var product = _productRepo.GetProductById(id);
            if (product != null)
            {
                return true;
            }
            return false;
        }
    }
}
