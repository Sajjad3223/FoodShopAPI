using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrders();

        Task<Order> GetOrderById(int id);

        Task<Order> GetOrderByUserId(string userId);

        Task<Order> GetUserOpenOrder(string userId);

        Task<bool> InsertOrder(Order order);

        Task<bool> UpdateOrder(Order order);

        Task<bool> DeleteOrder(Order order);

        Task<bool> DeleteOrder(int id);
    }
}