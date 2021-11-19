using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class Order : BaseEntity
    {
        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Required]
        public int TotalPrice { get; set; }

        [Required]
        public string UserId { get; set; }

        public bool IsPaid { get; set; } = false;

        [ForeignKey("UserId")]
        public ApiUser User { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}