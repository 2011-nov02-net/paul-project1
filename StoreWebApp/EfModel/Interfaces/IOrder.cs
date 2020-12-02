using System;
using System.Collections.Generic;
using System.Text;
using StoreLibrary;

namespace EfModel.Interfaces
{
    public interface IOrderRepo
    {
        public void AddOrder(Order order);
        public List<Order> GetAllOrders();
        public Order GetOrderById(int orderId);
        public List<Order> GetOrdersByStore(Store store);
        public List<Order> GetOrdersByCustomer(Customer customer);

        //Order Items or just Items
        public void AddItem(OrderItem orderItem);
        public OrderItem GetItemById(int orderItemId);
        public List<OrderItem> GetItemsByOrder(Order order);

    }
}
