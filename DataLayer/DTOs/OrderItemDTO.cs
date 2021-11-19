using System.ComponentModel.DataAnnotations;

namespace DataLayer.DTOs
{
    public class CreateOrderItemDTO
    {
        [Required]
        public int Count { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int FoodId { get; set; }
        [Required]
        public int OrderId { get; set; }
    }

    public class OrderItemDTO : CreateOrderItemDTO
    {
        public int Id { get; set; }

        public OrderDTO Order { get; set; }

        public FoodDTO Food { get; set; }

    }
}