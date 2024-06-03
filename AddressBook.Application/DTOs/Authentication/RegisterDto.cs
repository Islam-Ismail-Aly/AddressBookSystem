using AddressBook.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace AddressBook.Application.DTOs.Authentication
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [ComparePasswords("Password")]
        public string ConfirmPassword { get; set; }
    }
}
