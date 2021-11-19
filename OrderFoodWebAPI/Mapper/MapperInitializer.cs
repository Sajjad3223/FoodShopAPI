using AutoMapper;
using DataLayer.DTOs;
using DataLayer.Entities;

namespace OrderFoodWebAPI.Mapper
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Food, FoodDTO>().ReverseMap();
            CreateMap<Food, CreateFoodDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Order, CreateOrderDTO>().ReverseMap();
            CreateMap<OrderItem, CreateOrderItemDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<ApiUser, UserDTO>().ReverseMap();
        }
    }
}