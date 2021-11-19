using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using DataLayer.DTOs;
using AutoMapper;
using System.Linq;

namespace OrderFoodWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IFoodService _foodService;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IOrderItemService orderItemService, UserManager<ApiUser> userManager, IMapper mapper, IFoodService foodService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
            _userManager = userManager;
            _mapper = mapper;
            _foodService = foodService;
        }

        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderById(orderId);
                var orderDto = _mapper.Map<OrderDTO>(order);
                var items = await _orderItemService.GetOrderItemsByOrderId(orderId);
                orderDto.OrderItems = _mapper.Map<List<OrderItemDTO>>(items);
                orderDto.TotalPrice = items.Sum(i => i.Price * i.Count);

                return Ok(orderDto);
            }
            catch (Exception e)
            {
                return Problem(e.Message + "   " + e.InnerException?.Message, statusCode: 500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOpenOrder()
        {
            try
            {
                ApiUser user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Name).Value);

                var order = await _orderService.GetUserOpenOrder(user.Id);
                if (order == null)
                    return NoContent();

                var orderDto = _mapper.Map<OrderDTO>(order);
                var items = await _orderItemService.GetOrderItemsByOrderId(order.Id);
                orderDto.OrderItems = _mapper.Map<List<OrderItemDTO>>(items);
                orderDto.TotalPrice = items.Sum(i => i.Price * i.Count);

                return Ok(orderDto);
            }
            catch (Exception e)
            {
                return Problem(e.Message + "   " + e.InnerException?.Message, statusCode: 500);
            }
        }

        [HttpGet("AddToCart/{foodId:int}/{count:int}")]
        public async Task<IActionResult> AddItemToCart(int foodId,int count = 1)
        {
            try
            {
                if (foodId < 1)
                    return NotFound();

                var food = await _foodService.GetFoodById(foodId);
                if (food == null)
                    return NotFound();

                ApiUser user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Name).Value);

                var order = await _orderService.GetUserOpenOrder(user.Id);

                if (order == null)
                {
                    CreateOrderDTO orderDto = new CreateOrderDTO() { UserId = user.Id };
                    order = _mapper.Map<Order>(orderDto);
                    await _orderService.InsertOrder(order);
                }

                var itemIncart = await _orderItemService.DoesOrderHasItem(order.Id, foodId);

                if (itemIncart != null)
                {
                    itemIncart.Count += count;
                    await _orderItemService.UpdateOrderItem(itemIncart);
                }
                else
                {
                    CreateOrderItemDTO itemDto = new CreateOrderItemDTO
                    {
                        FoodId = foodId,
                        Count = count,
                        OrderId = order.Id,
                        Price = food.Price
                    };

                    var orderItem = _mapper.Map<OrderItem>(itemDto);

                    await _orderItemService.InsertOrderItem(orderItem);
                }


                return await GetOrder(order.Id);
            }
            catch (Exception e)
            {
                return Problem(e.Message + "   " + e.InnerException?.Message, statusCode: 500);
            }
        }
        
        [HttpDelete("{itemId:int}")]
        public async Task<IActionResult> DeleteOrderItem(int itemId)
        {
            try
            {
                var item = await _orderItemService.GetOrderItemById(itemId);
                if (item == null)
                    return NotFound();

                await _orderItemService.DeleteOrderItem(itemId);

                return NoContent();
            }
            catch (Exception e)
            {
                return Problem(e.Message + "   " + e.InnerException?.Message, statusCode: 500);
            }
        }
    }
}
