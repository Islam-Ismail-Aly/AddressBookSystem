using AddressBook.Application.DTOs.Users;
using AddressBook.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace AddressBook.Application.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
    }
}
