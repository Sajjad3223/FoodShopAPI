using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Food : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FoodName { get; set; }

        [MaxLength(100)]
        public string ImageName { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
