using EfModel.Interfaces;
using EfModel.Models;
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
using Inventory = StoreLibrary.Inventory;
using Order = StoreLibrary.Order;
using Product = StoreLibrary.Product;
using Store = StoreLibrary.Store;

namespace StoreWebApp.Controllers
{
    public class StoresController : Controller
    {
        private readonly IStore _storeRepo;
        private readonly IProduct _productRepo;
        private readonly IOrder _orderRepo;
        private readonly ILogger<StoresController> _logger;
        private readonly project0Context _context;
        public StoresController(ILogger<StoresController> logger, IStore storeRepo, IProduct productRepo, IOrder orderRepo,project0Context context)
        {
            _logger = logger;
            _storeRepo = storeRepo;
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _context = context;     
        }

        // GET: StoresController
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stores.ToListAsync());
        }

        // GET: Stores1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Stores
                .FirstOrDefaultAsync(m => m.StoreId == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
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
                var productCheck = ProductExists(inventory.ProductName);
                if (productCheck != false) 
                {
                    var product = _productRepo.GetProductByName(inventory.ProductName);
                    var existInventory = location.Inventory.Any(i => i.ProductName == inventory.ProductName);
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


        // GET: Stores1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            return View(store);
        }

        // POST: Stores1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StoreId,StoreName,Date")] Store store)
        {
            if (id != store.StoreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(store);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.StoreId))
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
            var result = _orderRepo.GetOrdersByStore(location);
            return View(result);
        }

        // GET: Stores1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Stores
                .FirstOrDefaultAsync(m => m.StoreId == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(int id)
        {
            return _context.Stores.Any(e => e.StoreId == id);
        }


        private bool StoreExists(string storeName)
        {
            bool exist = (_storeRepo.GetLocationByName(storeName) != null);
            return exist;
        }

        public bool ProductExists(string productName)
        {
            var product = _productRepo.GetProductByName(productName);
            if (product != null)
            {
                return true;
            }
            return false;
        }
    }
}
