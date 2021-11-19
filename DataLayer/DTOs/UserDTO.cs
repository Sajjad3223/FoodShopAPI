using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.DTOs
{
    public class UserDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(15,ErrorMessage = "پسورد وارد شده باید بین 6 تا 15 کاراکتر باشد" ,MinimumLength = 6)]
        public string Password { get; set; }

        public string Address { get; set; } = "";

        public ICollection<string> Roles { get; set; }
    }
}