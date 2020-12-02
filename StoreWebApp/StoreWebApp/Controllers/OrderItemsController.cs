using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repositories.EfModel;
using StoreWebApp.EfModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly project0Context _context;
        private IOrderRepo _order;
        private IStoreRepo _store;
        private ICustomerRepo _customer;
        private IProductRepo _product;

        public OrderItemsController(project0Context context, IOrderRepo order, IStoreRepo store, ICustomerRepo customer, IProductRepo product)
        {
            _context = context;
            _order = order;
            _store = store;
            _customer = customer;
            _product = product;
        }
        //Get Orders
        public ActionResult Index()
        {
            var project0Context = _context.Orders.Include(o => o.Customer).Include(o => o.Store);
            return View(project0Context.ToList());
     
        }
        //Get Order/Details/CustomerId
        public ActionResult Details(int id)
        {
            var result = _order.GetOrdersByCustomer(id);
            return View(result);
        }

        //Get Order/Create
        public ActionResult Create()
        {
            var result = new Order();
            result.OrderId = 0;
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FirstName");
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "StoreName");
            result.OrderTotalPrice = 0;
            return View();
        }
        //POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,StoreId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FirstName", order.CustomerId);
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "StoreName", order.StoreId);
            return View(order);
        }
        //GET Items/Create
        public ActionResult AddItems(int id)
        {
            var result = new OrderItem();
            result.OrderId = id;
            result.Quantity = 1;
            var currentOrder = _order.GetOrdersById(id);
            var currentInventory = _store.GetInventories(currentOrder.Store);
            var product = _product.GetAllProducts();
            foreach(var item in product)
            {
                if(currentInventory.Any(a => (a.ProductName == item.ProductName && a.Stock > 0)))
                {
                    result.Products.Add(item);
                }
            }
            return View(result);
        }

    }
}
