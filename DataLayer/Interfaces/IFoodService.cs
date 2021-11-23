using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.DTOs;
using DataLayer.Entities;

namespace DataLayer.Interfaces
{
    public interface IFoodService
    {
        Task<List<Food>> GetAllFoods();
        Task<List<Food>> SearchInFoods(string q);

        Task<Food> GetFoodById(int id);

        Task<bool> InsertFood(Food food);

        Task<bool> UpdateFood(Food food);

        Task<bool> DeleteFood(Food food);

        Task<bool> DeleteFood(int id);
    }
}