using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DTOs;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace OrderFoodWebAPI.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;

        private ApiUser user;

        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> ValidateUser(LoginDTO loginDto)
        {
            user = await _userManager.FindByNameAsync(loginDto.Email.ToLower().Trim());
            return (user != null && await _userManager.CheckPasswordAsync(user,loginDto.Password));
        }

        public async Task<string> CreateToken()
        {
            var signinCredentials = GetSigninCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signinCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials credentials, List<Claim> claimsList)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("LifeTime").Value));

            var token = new JwtSecurityToken(
                issuer:jwtSettings.GetSection("Issuer").Value,
                claims:claimsList,
                expires:expiration,
                signingCredentials: credentials
            );

            return token;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role , role));
            }

            return claims;
        }

        private SigningCredentials GetSigninCredentials()
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = jwtSettings.GetSection("KEY").Value;
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret,SecurityAlgorithms.HmacSha256);
        }
    }
}