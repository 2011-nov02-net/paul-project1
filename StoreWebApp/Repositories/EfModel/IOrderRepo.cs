using StoreWebApp.EfModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.EfModel
{
    public interface IOrderRepo
    {
        public void UpdateOrder(OrderItem orderItem);
        public void AddItem(ICollection<OrderItem> items, Store store);

        public void AddOrder(Customer customer, Store store, ICollection<OrderItem> items);

        public List<Order> GetAllOrders();

        public List<Order> GetOrdersByCustomer(int customerId);

        public Order GetOrdersById(int orderId);
        public List<OrderItem> GetOrderItemsByOrder(Order order);

    }
}
