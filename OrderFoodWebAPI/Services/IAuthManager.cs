using System.Threading.Tasks;
using DataLayer.DTOs;

namespace OrderFoodWebAPI.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginDTO loginDto);

        Task<string> CreateToken();
    }
}