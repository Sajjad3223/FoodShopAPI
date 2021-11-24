using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly FoodShopContext _context;

        public OrderService(FoodShopContext context)
        {
            _context = context;
        }

        public Task<List<Order>> GetAllOrders()
        {
            return _context.Orders.ToListAsync();
        }

        public Task<Order> GetOrderById(int id)
        {
            return _context.Orders.AsNoTracking().Include(o=>o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
        }

        public Task<Order> GetOrderByUserId(string userId)
        {
            return _context.Orders.AsNoTracking().Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.UserId == userId);
        }

        public Task<Order> GetUserOpenOrder(string userId)
        {
            return _context.Orders.AsNoTracking().Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.UserId == userId && !o.IsPaid);
        }

        public async Task<bool> InsertOrder(Order order)
        {
            try
            {
                await _context.AddAsync(order);
                await _context.SaveChangesAsync();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<bool> DeleteOrder(Order order)
        {
            try
            {
                _context.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await GetOrderById(id);
            return await DeleteOrder(order);
        }

    }
}