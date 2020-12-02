using Microsoft.EntityFrameworkCore;
using Repositories.EfModel;
using StoreWebApp.EfModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class OrderRepo:IOrderRepo
    {
        private readonly DbContextOptions<project0Context> _contextOptions;
        private CustomerRepo _customer;
        private StoreRepo _store;
        public OrderRepo(DbContextOptions<project0Context> contextOptions)
        {
            _contextOptions = contextOptions;
            _store = new StoreRepo(contextOptions);
            _customer = new CustomerRepo(contextOptions);
        }
        //Update Order Item
        public void UpdateOrder(OrderItem orderItem)
        {
            using var context = new project0Context(_contextOptions);
            var dbOrderItem = context.OrderItems
                .Where(oi => oi.OrderId == orderItem.OrderId)
                .FirstOrDefault();
            dbOrderItem.OrderId = orderItem.OrderId;
            dbOrderItem.ProductId = orderItem.ProductId;
            dbOrderItem.Quantity = orderItem.Quantity;
            context.SaveChanges();
        }
        //Add OrderItem
        public void AddItem(ICollection<OrderItem> items, Store store)
        {
            using var context = new project0Context(_contextOptions);
            var dbItems = new List<OrderItem>();
            decimal orderTotal = 0.0m;
            foreach (var item in items)
            {
                var dbProduct = context.Products.First(p => p.ProductId == item.ProductId);
                var dbItem = new OrderItem()
                {
                    ProductId = item.ProductId,
                    PurchasePrice = dbProduct.Price,
                    Quantity = item.Quantity
                };
                orderTotal += item.Quantity * dbProduct.Price;
                dbItems.Add(dbItem);
                var inventory = context.Inventories.First(i => i.StoreId == store.StoreId && i.ProductId == item.ProductId);
                inventory.Stock -= item.Quantity;
                context.Inventories.Update(inventory);
                context.SaveChanges();
            }
        }

        //Add Order
        public void AddOrder(Customer customer, Store store, ICollection<OrderItem> items)
        {
            using var context = new project0Context(_contextOptions);
            var dbItems = new List<OrderItem>();
            decimal orderTotal = 0.0m;
            foreach (var item in items)
            {
                var dbProduct = context.Products.First(p => p.ProductId == item.ProductId);
                var dbItem = new OrderItem()
                {
                    ProductId = item.ProductId,
                    PurchasePrice = dbProduct.Price,
                    Quantity = item.Quantity
                };
                orderTotal += item.Quantity * dbProduct.Price;
                dbItems.Add(dbItem);
                var inventory = context.Inventories.First(i => i.StoreId == store.StoreId && i.ProductId == item.ProductId);
                inventory.Stock -= item.Quantity;
                context.Inventories.Update(inventory);
                context.SaveChanges();
            }
            var order = new Order()
            {
                CustomerId = customer.CustomerId,
                StoreId = store.StoreId,
                Date = DateTime.Now,
                OrderItems = dbItems

            };
            order.OrderTotalPrice = orderTotal;
            context.Orders.Add(order);
            context.SaveChanges();
        }
       
        //Get Order by Customer
        public List<Order> GetOrdersByCustomer(int customerId)
        {
            using var context = new project0Context(_contextOptions);
            var dbCustomerOrders = context.Orders.Where(o => o.CustomerId == customerId).ToList();
            return dbCustomerOrders.Select(o => new Order(o.OrderId, o.CustomerId, o.StoreId, o.OrderTotalPrice,o.OrderItems, o.Date)).ToList();
        }
        //Get Order by Store
        public List<Order> GetOrdersByStore(int storeId)
        {
            using var context = new project0Context(_contextOptions);
            var dbStoreOrders = context.Orders.Where(o => o.StoreId == storeId).ToList();
            return dbStoreOrders.Select(o => new Order(o.OrderId, o.CustomerId, o.StoreId, o.OrderTotalPrice, o.Date)).ToList();
        }
        //Get All Orders
        public List<Order> GetAllOrders()
        {
            using var context = new project0Context(_contextOptions);
            var dbOrders = context.Orders.ToList();
            return dbOrders;
        }

        public Order GetOrdersById(int orderId)
        {
            using var context = new project0Context(_contextOptions);
            var dbOrder = context.Orders
                .Where(o => o.OrderId == orderId)
                .FirstOrDefault();
            var result = new Order()
            {
                OrderId = dbOrder.OrderId,
                Customer = _customer.GetCustomerById(dbOrder.CustomerId),
                Store = _store.GetAllStoresById(dbOrder.StoreId),
                OrderTotalPrice = dbOrder.OrderTotalPrice,
                Date = dbOrder.Date
            };
            var orderItems = GetOrderItemsByOrder(result);
            foreach (var item in orderItems)
            {
                result.OrderItems.Add(item);
            }
            return result;
        }

        public List<OrderItem> GetOrderItemsByOrder(Order order)
        {
            using var context = new project0Context(_contextOptions);
            var dbItems = context.OrderItems
                .Where(i => i.OrderId == order.OrderId)
                .Include(i => i.Product)
                .ToList();
            var result = new List<OrderItem>();
            foreach(var item in dbItems)
            {
                var product = new Product()
                {
                    ProductId = item.Product.ProductId,
                    ProductName = item.Product.ProductName,
                    Price = item.Product.Price

                };
                var orderItem = new OrderItem(item.OrderId, product, item.Quantity, item.PurchasePrice);
                orderItem.OrderId = item.OrderId;
                result.Add(orderItem);
            }
            return result;
        }
    }
}
