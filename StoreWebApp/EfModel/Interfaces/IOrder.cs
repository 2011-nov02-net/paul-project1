using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfModel.Interfaces
{
    public interface IOrder
    {
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
        List<Order> GetOrdersByStore(Store store);
        List<Order> GetOrdersByCustomer(Customer customer);
        void CreateOrder(Order order);
        Order ReturnOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);

        //Items
        void CreateOrderItem(OrderItem orderItem);
        List<OrderItem> GetOrderItemsByOrder(Order order);
        OrderItem GetOrderItemById(int id);
        void UpdateOrderItem(OrderItem orderItem);
    }
}
