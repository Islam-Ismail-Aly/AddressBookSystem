using AddressBook.Application.DTOs.Dashboard;
using AddressBook.Application.Interfaces.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "DashboardAPIv1")]
    [Produces("application/json")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("GetDashboardData")]
        public async Task<ActionResult<DashboardDto>> GetDashboardData()
        {
            var result = await _service.GetDashboardDataAsync();
            return Ok(result);
        }
    }
}
