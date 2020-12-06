using EfModel.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreLibrary;
using StoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProduct _productRepo;
        public ProductsController(IProduct productRepo)
        {
            _productRepo = productRepo;
        }

        // GET: ProductsController
        public IActionResult Index()
        {
            var products = _productRepo.GetAllProducts();
            var result = new List<ProductViewModel>();
            foreach (var product in products)
            {
                var newProduct = new ProductViewModel()
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price
                };
                result.Add(newProduct);
            }
            return View(result);
        }

        // GET: ProductsController/Details/5
        public IActionResult Details()
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Controller Error!");
            }
            return View();
        }

        // GET: ProductsController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            try
            {
                _productRepo.CreateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Edit/5
        public IActionResult Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Controller Error!");
            }
            return View();
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw new Exception("Controller Error!");
            }
        }

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Product product)
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
    }
}
