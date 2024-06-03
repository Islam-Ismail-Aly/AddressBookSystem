using AddressBook.Application.DTOs.Authentication;
using AddressBook.Application.Interfaces.Authentication;
using AddressBook.Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "AuthenticationAPIv1")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountAuthenticationService _authenticationService;

        public AccountController(IAccountAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            var result = await _authenticationService.LoginAsync(loginDto);

            if (!result.IsAuthenticated)
                return BadRequest(new { result.Message, result.IsAuthenticated });

            return Ok(new APIResponseResult<AuthenticationDto>(result, "Login successfully."));
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model)
        {
            var result = await _authenticationService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponseResult<JsonContent>(ModelState.ToString()));

            return Ok(new { token = result.Token, result.ExpiresOn });
        }
    }
}
