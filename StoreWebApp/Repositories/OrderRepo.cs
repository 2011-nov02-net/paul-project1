using Microsoft.EntityFrameworkCore;
using StoreWebApp.EfModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class OrderRepo
    {
        private readonly DbContextOptions<project0Context> _contextOptions;
        public OrderRepo(DbContextOptions<project0Context> contextOptions)
        {
            _contextOptions = contextOptions;
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
            //dbOrderItem.Total = orderItem.Total;
            context.SaveChanges();
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
        public ICollection<Order> GetOrdersByCustomer(int customerId)
        {
            using var context = new project0Context(_contextOptions);
            var dbCustomerOrders = context.Orders.Where(o => o.CustomerId == customerId).ToList();
            return dbCustomerOrders.Select(o => new Order(o.OrderId, o.CustomerId, o.StoreId, o.OrderTotalPrice,o.OrderItems, o.Date)).ToList();
        }
        //Get Order by Store
        public ICollection<Order> GetOrdersByStore(int storeId)
        {
            using var context = new project0Context(_contextOptions);
            var dbStoreOrders = context.Orders.Where(o => o.StoreId == storeId).ToList();
            return dbStoreOrders.Select(o => new Order(o.OrderId, o.CustomerId, o.StoreId, o.OrderTotalPrice, o.Date)).ToList();
        }
        //Get All Orders
        public ICollection<Order> GetAllOrders()
        {
            using var context = new project0Context(_contextOptions);
            var dbOrders = context.Orders.ToList();
            return dbOrders;
        }
    }
}
