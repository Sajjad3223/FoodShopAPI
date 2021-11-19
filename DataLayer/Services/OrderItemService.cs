using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly FoodShopContext _context;

        public OrderItemService(FoodShopContext context)
        {
            _context = context;
        }

        public Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId)
        {
            return _context.OrderItems.Where(oi=>oi.OrderId == orderId).ToListAsync();
        }

        public Task<OrderItem> GetOrderItemById(int id)
        {
            return _context.OrderItems.FirstOrDefaultAsync(oi => oi.Id == id);
        }

        public Task<OrderItem> DoesOrderHasItem(int orderId, int foodId)
        {
            return _context.OrderItems.FirstOrDefaultAsync(i => i.OrderId == orderId && i.FoodId == foodId);
        }

        public async Task<bool> InsertOrderItem(OrderItem orderItem)
        {
            try
            {
                await _context.AddAsync(orderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateOrderItem(OrderItem orderItem)
        {
            try
            {
                _context.Update(orderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteOrderItem(OrderItem orderItem)
        {
            try
            {
                _context.Remove(orderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteOrderItem(int id)
        {
            var orderItem = await GetOrderItemById(id);
            return await DeleteOrderItem(orderItem);
        }


    }
}