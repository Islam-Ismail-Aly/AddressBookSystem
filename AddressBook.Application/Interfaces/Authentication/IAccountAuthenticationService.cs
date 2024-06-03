using AddressBook.Application.DTOs.Authentication;

namespace AddressBook.Application.Interfaces.Authentication
{
    public interface IAccountAuthenticationService
    {
        Task<AuthenticationDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthenticationDto> LoginAsync(LoginDto loginDto);
    }
}
