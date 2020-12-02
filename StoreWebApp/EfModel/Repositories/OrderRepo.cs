using EfModel.EfModel;
using EfModel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoreLibrary;

namespace EfModel.Repositories
{
    public class OrderRepo : IOrderRepo
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
        public void AddOrder(StoreLibrary.Order order)
        {
            using var context = new project0Context(_contextOptions);
            var neworder = new EfModel.Order()
            {
                CustomerId = order.Customer.CustomerId,
                StoreId = order.Store.StoreId,
                Date = order.Date,
                OrderTotalPrice = order.OrderTotalPrice
            };
            context.Orders.Add(neworder);
            context.SaveChanges();
            foreach (var orderItem in order.OrderItems)
            {
                orderItem.OrderId = neworder.OrderId;
                AddItem(orderItem);
            }
            context.SaveChanges();
        }

        public List<StoreLibrary.Order> GetAllOrders()
        {
            using var context = new project0Context(_contextOptions);
            var dbOrders = context.Orders
                .Include(o => o.Store)
                .Include(o => o.Customer)
                .ToList();
            var result = new List<StoreLibrary.Order>();
            foreach (var order in dbOrders)
            {
                var newStore = _store.GetStoreById(order.StoreId);
                var newCust = _customer.GetCustomerById(order.CustomerId);
                var newOrder = new StoreLibrary.Order()
                {
                    OrderId = order.OrderId,
                    Store = newStore,
                    Customer = newCust,
                    OrderTotalPrice = order.OrderTotalPrice,
                    Date = order.Date
                };
                result.Add(newOrder);
            };
            return result;
        }

        public StoreLibrary.Order GetOrderById(int orderId)
        {
            using var context = new project0Context(_contextOptions);
            var dbOrder = context.Orders
                .Where(l => l.OrderId == orderId)
                .FirstOrDefault();
            var result = new StoreLibrary.Order()
            {
                OrderId = dbOrder.OrderId,
                Store = _store.GetStoreById(dbOrder.StoreId),
                Customer = _customer.GetCustomerById(dbOrder.CustomerId),
                OrderTotalPrice = dbOrder.OrderTotalPrice,
                Date = dbOrder.Date
            };
            var orderItems = GetItemsByOrder(result);
            foreach (var items in orderItems)
            {
                result.OrderItems.Add(items);
            }
            return result;
        }

        public List<StoreLibrary.Order> GetOrdersByCustomer(StoreLibrary.Customer customer)
        {
            using var context = new project0Context(_contextOptions);
            var dbOrders = context.Orders
                .Where(o => o.CustomerId == customer.CustomerId)
                .ToList();
            var result = new List<StoreLibrary.Order>();
            foreach (var order in dbOrders)
            {
                var newLocation = _store.GetStoreById(order.StoreId);
                var newCustomer = _customer.GetCustomerById(order.CustomerId);
                var newOrder = new StoreLibrary.Order()
                {
                    OrderId = order.OrderId,
                    Store = newLocation,
                    Customer = newCustomer
                };
                newOrder.OrderId = order.OrderId;
                newOrder.Date = order.Date;
                var newOrderItems = GetItemsByOrder(newOrder);
                foreach (var orderItem in newOrderItems)
                {
                    newOrder.OrderItems.Add(orderItem);
                }
                result.Add(newOrder);
            }
            return result;
        }

        public List<StoreLibrary.Order> GetOrdersByStore(StoreLibrary.Store store)
        {
            using var context = new project0Context(_contextOptions);
            var custRepo = new CustomerRepo(_contextOptions);
            var dbOrders = context.Orders
                .Where(o => o.StoreId == store.StoreId)
                .Include(o => o.Store)
                .Include(o => o.Customer)
                .ToList();
            var result = new List<StoreLibrary.Order>();
            foreach (var order in dbOrders)
            {
                var newLocation = _store.GetStoreById(order.StoreId);
                var newCust = _customer.GetCustomerById(order.CustomerId);
                var newOrder = new StoreLibrary.Order()
                {
                    OrderId = order.OrderId,
                    Store = newLocation,
                    Customer = newCust
                };
                newOrder.OrderId = order.OrderId;
                newOrder.Date = order.Date;
                var newOrderItems = GetItemsByOrder(newOrder);
                foreach (var orderItem in newOrderItems)
                {
                    newOrder.OrderItems.Add(orderItem);
                }
                result.Add(newOrder);
            }
            return result;
        }

        //Items

        public void AddItem(StoreLibrary.OrderItem orderItem)
        {
            using var context = new project0Context(_contextOptions);
            var _orderItem = new EfModel.OrderItem()
            {
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity,
                PurchasePrice = orderItem.PurchasePrice
            };
            context.OrderItems.Add(_orderItem);
            context.SaveChanges();
        }

        public StoreLibrary.OrderItem GetItemById(int orderItemId)
        {
            using var context = new project0Context(_contextOptions);
            var dbOrderItem = context.OrderItems
                .Where(o => o.ItemId == orderItemId)
                .Include(o => o.Product)
                .FirstOrDefault();
            var product = new StoreLibrary.Product()
            {
                ProductId = dbOrderItem.ProductId,
                ProductName = dbOrderItem.Product.ProductName,
                Price = dbOrderItem.Product.Price
            };
            var result = new StoreLibrary.OrderItem(dbOrderItem.OrderId, product, dbOrderItem.Quantity, dbOrderItem.PurchasePrice);
            result.ItemId = dbOrderItem.ItemId;
            return result;
        }

        public List<StoreLibrary.OrderItem> GetItemsByOrder(StoreLibrary.Order order)
        {
            using var context = new project0Context(_contextOptions);
            var dbOrderItems = context.OrderItems
                .Where(o => o.OrderId == order.OrderId)
                .Include(o => o.Product)
                .ToList();
            var result = new List<StoreLibrary.OrderItem>();
            foreach (var items in dbOrderItems)
            {
                var product = new StoreLibrary.Product()
                {
                    ProductId = items.ProductId,
                    ProductName = items.Product.ProductName,
                    Price = items.Product.Price
                };
                var newOrderItem = new StoreLibrary.OrderItem(items.OrderId, product, items.Quantity, items.PurchasePrice);
                newOrderItem.ItemId = items.ItemId;
                result.Add(newOrderItem);
            }
            return result;
        }
    }
}
