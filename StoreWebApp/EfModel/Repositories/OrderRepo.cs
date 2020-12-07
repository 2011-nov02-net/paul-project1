using EfModel.Interfaces;
using EfModel.Models;
using Microsoft.EntityFrameworkCore;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Customer = StoreLibrary.Customer;
using Order = StoreLibrary.Order;
using OrderItem = StoreLibrary.OrderItem;
using Product = StoreLibrary.Product;
using Store = StoreLibrary.Store;

namespace EfModel.Repositories
{
    public class OrderRepo : IOrder
    {
        private readonly DbContextOptions<project0Context> _contextOptions;
        private readonly CustomerRepo _customerRepo;
        private readonly StoreRepo _storeRepo;
        public OrderRepo(DbContextOptions<project0Context> contextOptions)
        {
            _contextOptions = contextOptions;
            _storeRepo = new StoreRepo(contextOptions);
            _customerRepo = new CustomerRepo(contextOptions);
        }
        public void CreateOrder(Order order)
        {
            using var context = new project0Context(_contextOptions);
            var orderEntry = new Models.Order()
            {
                CustomerId = order.Customer.CustomerId,
                StoreId = order.Store.StoreId,
                Date = order.Date,
                OrderTotal= order.OrderTotalPrice
            };
            context.Orders.Add(orderEntry);
            context.SaveChanges();
            foreach (var orderItem in order.OrderItems)
            {
                orderItem.OrderId = orderEntry.OrderId;
                CreateOrderItem(orderItem);
            }
            context.SaveChanges();
        }

        public void CreateOrderItem(OrderItem orderItem)
        {
            using var context = new project0Context(_contextOptions);
            var orderItemEntry = new Models.OrderItem()
            {
                OrderId = orderItem.OrderId,
                ProductId = orderItem.Product.ProductId,
                Quantity = orderItem.Quantity,
                Total = orderItem.PurchasePrice
            };
            context.OrderItems.Add(orderItemEntry);
            context.SaveChanges();
        }

        public void DeleteOrder(Order order)
        {
            using var context = new project0Context(_contextOptions);
            var dbOrder = context.Orders
                .Where(i => i.OrderId == order.OrderId)
                .FirstOrDefault();
            context.Remove(dbOrder);
            context.SaveChanges();
        }

        public List<Order> GetAllOrders()
        {
            using var context = new project0Context(_contextOptions);
            var dbOrders = context.Orders
                .Include(o => o.Store)
                .Include(o => o.Customer)
                .ToList();
            var result = new List<Order>();
            foreach (var order in dbOrders)
            {
                var newStore = _storeRepo.GetStoreById(order.StoreId);
                var newCust = _customerRepo.GetCustomerById(order.CustomerId);
                var newOrder = new Order()
                {
                    OrderId = order.OrderId,
                    Store = newStore,
                    Customer = newCust,
                    OrderTotalPrice = order.OrderTotal,
                    Date = order.Date
                };
                result.Add(newOrder);
            };
            return result;
        }

        public Order GetOrderById(int id)
        {
            using var context = new project0Context(_contextOptions);
            var dbOrder = context.Orders
                .Where(l => l.OrderId == id)
                .FirstOrDefault();
            if (dbOrder == null)
            {
                return null;
            }
            var result = new Order()
            {
                OrderId = dbOrder.OrderId,
                Store = _storeRepo.GetStoreById(dbOrder.StoreId),
                Customer = _customerRepo.GetCustomerById(dbOrder.CustomerId),
                OrderTotalPrice = dbOrder.OrderTotal,
                Date = dbOrder.Date
            };
            var orderItems = GetOrderItemsByOrder(result);
            foreach (var thing in orderItems)
            {
                result.OrderItems.Add(thing);
            }
            return result;
        }

        public OrderItem GetOrderItemById(int id)
        {
            using var context = new project0Context(_contextOptions);
            var dbOrderItem = context.OrderItems
                .Where(o => o.ItemId == id)
                .Include(o => o.Product)
                .FirstOrDefault();
            if (dbOrderItem == null)
            {
                return null;
            }
            var newAnimal = new Product()
            {
                ProductId = dbOrderItem.Product.ProductId,
                ProductName = dbOrderItem.Product.ProductName,
                Price = dbOrderItem.Product.Price
            };
            var result = new OrderItem(dbOrderItem.OrderId, newAnimal, dbOrderItem.Quantity, (decimal)dbOrderItem.Total)
            {
                ItemId = dbOrderItem.ItemId
            };
            return result;
        }

        public List<OrderItem> GetOrderItemsByOrder(Order order)
        {
            using var context = new project0Context(_contextOptions);
            var dbOrderItems = context.OrderItems
                .Where(o => o.OrderId == order.OrderId)
                .Include(o => o.Product)
                .ToList();
            var result = new List<OrderItem>();
            foreach (var orderItem in dbOrderItems)
            {
                var newProduct = new Product()
                {
                    ProductId = orderItem.Product.ProductId,
                    ProductName = orderItem.Product.ProductName,
                    Price = orderItem.Product.Price
                };
                var newOrderItem = new OrderItem(orderItem.OrderId, newProduct, orderItem.Quantity, orderItem.Total)
                {
                    ItemId = orderItem.ItemId
                };
                result.Add(newOrderItem);
            }
            return result;
        }

        public List<Order> GetOrdersByCustomer(Customer customer)
        {
            using var context = new project0Context(_contextOptions);
            var dbOrders = context.Orders
                .Where(o => o.CustomerId == customer.CustomerId)
                .ToList();
            var result = new List<Order>();
            foreach (var order in dbOrders)
            {
                var newLocation = _storeRepo.GetStoreById(order.StoreId);
                var newCust = _customerRepo.GetCustomerById(order.CustomerId);
                var newOrder = new Order()
                {
                    OrderId = order.OrderId,
                    Store = newLocation,
                    Customer = newCust,
                    OrderTotalPrice = order.OrderTotal
                };
                newOrder.OrderId = order.OrderId;
                newOrder.Date = order.Date;
                var newOrderItems = GetOrderItemsByOrder(newOrder);
                foreach (var orderItem in newOrderItems)
                {
                    newOrder.OrderItems.Add(orderItem);
                }
                result.Add(newOrder);
            }
            return result;
        }

        public List<Order> GetOrdersByStore(Store store)
        {
            using var context = new project0Context(_contextOptions);
            var custRepo = new CustomerRepo(_contextOptions);
            var dbOrders = context.Orders
                .Where(o => o.StoreId == store.StoreId)
                .Include(o => o.Store)
                .Include(o => o.Customer)
                .ToList();
            var result = new List<Order>();
            foreach (var order in dbOrders)
            {
                var newLocation = _storeRepo.GetStoreById(order.StoreId);
                var newCust = _customerRepo.GetCustomerById(order.CustomerId);
                var newOrder = new Order()
                {
                    OrderId = order.OrderId,
                    Store = newLocation,
                    Customer = newCust,
                    OrderTotalPrice = order.OrderTotal
                };
                newOrder.OrderId = order.OrderId;
                newOrder.Date = order.Date;
                var newOrderItems = GetOrderItemsByOrder(newOrder);
                foreach (var orderItem in newOrderItems)
                {
                    newOrder.OrderItems.Add(orderItem);
                }
                result.Add(newOrder);
            }
            return result;
        }

        public Order ReturnOrder(Order order)
        {
            using var context = new project0Context(_contextOptions);
            var orderEntry = new Models.Order()
            {
                CustomerId = order.Customer.CustomerId,
                StoreId = order.Store.StoreId,
                Date = order.Date,
                OrderTotal = order.OrderTotalPrice
            };
            context.Orders.Add(orderEntry);
            context.SaveChanges();
            foreach (var orderItem in order.OrderItems)
            {
                orderItem.OrderId = orderEntry.OrderId;
                CreateOrderItem(orderItem);
            }
            context.SaveChanges();
            return GetOrderById(orderEntry.OrderId);
        }

        public void UpdateOrder(Order order)
        {
            using var context = new project0Context(_contextOptions);
            var dbOrder = context.Orders
                .Where(o => o.OrderId == order.OrderId)
                .FirstOrDefault();
            dbOrder.OrderTotal = order.OrderTotalPrice;
            context.SaveChanges();
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            using var context = new project0Context(_contextOptions);
            var dbOrderItem = context.OrderItems
                .Where(o => o.ItemId == orderItem.ItemId)
                .FirstOrDefault();
            dbOrderItem.Quantity = orderItem.Quantity;
            dbOrderItem.Total = orderItem.PurchasePrice;
            context.SaveChanges();
        }
    }
}
