using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataLayer.DTOs;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using OrderFoodWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace OrderFoodWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public AccountController(UserManager<ApiUser> userManager, SignInManager<ApiUser> signInManager, IMapper mapper, IAuthManager authManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = userDTO.Email.ToLower().Trim();
                user.Address = "";
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code , error.Description);
                    }
                    return BadRequest(ModelState);
                }

                await _userManager.AddToRolesAsync(user, userDTO.Roles);
                return Accepted();
            }
            catch (Exception e)
            {
                return Problem(e.Message + "   " + e.InnerException?.Message, statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                if (!await _authManager.ValidateUser(loginDto))
                {
                    return Unauthorized(loginDto);
                }
                
                return Accepted(new {Token = await _authManager.CreateToken()});
            } 
            catch (Exception e)
            {
                return Problem(e.Message + "   " + e.InnerException?.Message, statusCode: 500);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            ApiUser user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Name).Value);

            var userDto = _mapper.Map<UserDTO>(user);

            userDto.Roles = await _userManager.GetRolesAsync(user);

            return Ok(userDto);
        }
    
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody]UpdateUserDTO userDto)
        {
            ApiUser user = await _userManager.FindByEmailAsync(userDto.Email);

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Address = userDto.Address;

            await _userManager.UpdateAsync(user);

            return Ok();
        }
    }
}
