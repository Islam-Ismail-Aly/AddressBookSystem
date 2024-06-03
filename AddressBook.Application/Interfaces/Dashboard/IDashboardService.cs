using AddressBook.Application.DTOs.Dashboard;

namespace AddressBook.Application.Interfaces.Dashboard
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync();
    }
}
