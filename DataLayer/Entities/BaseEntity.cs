using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}