using DataLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ResturantProgram.Informations;

namespace ResturantProgram
{
    public static class Informations
    {
        public enum EUserRole
        {
            User,
            Admin
        }

        public const string DOMAIN = "https://localhost:44317";

        public static string API_URL = $"{DOMAIN}/api";

        public static string FOODS_IMAGE_PATH = $"{DOMAIN}/images/";

        public static string Token = string.Empty;

        public static User User = new User();
    }

    public class User
    {
        public User()
        {
            this.FirstName = "";
            this.LastName = "";
            this.Email = "";
            this.PhoneNumber = "";
            this.Address = "";
        }
        public User(UserDTO userDto)
        {
            this.FirstName = userDto.FirstName ;
            this.LastName = userDto.LastName;
            this.Email= userDto.Email;
            this.PhoneNumber = userDto.PhoneNumber;
            this.Address = userDto.Address;
        }

        public User(UpdateUserDTO userDto)
        {
            this.FirstName = userDto.FirstName;
            this.LastName = userDto.LastName;
            this.Email = userDto.Email;
            this.PhoneNumber = userDto.PhoneNumber;
            this.Address = userDto.Address;
        }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public EUserRole UserRole { get; set; } = EUserRole.User;
    }
}
