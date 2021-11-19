using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class OrderItem : BaseEntity
    {
        [Required]
        public int Count { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int FoodId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("FoodId")]
        public Food Food { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}