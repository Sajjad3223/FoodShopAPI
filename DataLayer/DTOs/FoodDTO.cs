using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataLayer.Entities;

namespace DataLayer.DTOs
{
    public class CreateFoodDTO
    {
        [Required]
        [MaxLength(100)]
        public string FoodName { get; set; }

        [MaxLength(100)]
        public string ImageName { get; set; }

        [Required]
        public int Price { get; set; }
    }

    public class UpdateFoodDTO : CreateFoodDTO
    {
    }

    public class FoodDTO : CreateFoodDTO
    {
        public int Id { get; set; }
    }

}