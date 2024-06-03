using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AddressBook.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(30)]
        public string FirstName { get; set; }

        [Required, MaxLength(30)]
        public string LastName { get; set; }

        [Required, MaxLength(250)]
        public string Address { get; set; }
    }
}
