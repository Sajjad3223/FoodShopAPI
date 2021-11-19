using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataLayer.Entities;

namespace DataLayer.DTOs
{
    public class CreateOrderDTO
    {
        [Required]
        public string UserId { get; set; }

        public bool IsPaid { get; set; } = false;
    }

    public class OrderDTO : CreateOrderDTO
    {
        public int Id { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public int TotalPrice { get; set; }

        public UserDTO UserDto { get; set; }

        public IList<OrderItemDTO> OrderItems { get; set; }
    }
}