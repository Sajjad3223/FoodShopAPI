using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations
{
    public class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder.HasData(
                new Food()
                {
                    Id = 1,
                    FoodName = "پیتزا",
                    ImageName = "pitza.png",
                    Price = 25000
                },
                new Food()
                {
                    Id = 2,
                    FoodName = "جوجه",
                    ImageName = "jooje.png",
                    Price = 28000
                },
                new Food()
                {
                    Id = 3,
                    FoodName = "کباب کوبیده",
                    ImageName = "kabab.png",
                    Price = 26000
                }, 
                new Food()
                {
                    Id = 4,
                    FoodName = "برنج",
                    ImageName = "rice.png",
                    Price = 12000
                }
            );
        }
    }
}