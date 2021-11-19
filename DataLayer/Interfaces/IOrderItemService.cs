using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Interfaces
{
    public interface IOrderItemService
    {
        Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId);

        Task<OrderItem> GetOrderItemById(int id);

        Task<OrderItem> DoesOrderHasItem(int orderId,int foodId);

        Task<bool> InsertOrderItem(OrderItem orderItem);

        Task<bool> UpdateOrderItem(OrderItem orderItem);

        Task<bool> DeleteOrderItem(OrderItem orderItem);

        Task<bool> DeleteOrderItem(int id);
    }
}