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
    public class FoodService : IFoodService
    {
        private readonly FoodShopContext _context;

        public FoodService(FoodShopContext context)
        {
            _context = context;
        }

        public Task<List<Food>> GetAllFoods()
        {
            return _context.Foods.ToListAsync();
        }

        public Task<Food> GetFoodById(int id)
        {
            return _context.Foods.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool> InsertFood(Food food)
        {
            try
            {
                await _context.AddAsync(food);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateFood(Food food)
        {
            try
            {
                _context.Update(food);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteFood(Food food)
        {
            try
            {
                _context.Remove(food);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteFood(int id)
        {
            var food = await GetFoodById(id);
            return await DeleteFood(food);
        }
    }
}