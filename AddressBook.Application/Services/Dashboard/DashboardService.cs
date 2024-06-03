using AddressBook.Application.DTOs.Dashboard;
using AddressBook.Application.Interfaces;
using AddressBook.Application.Interfaces.Dashboard;
using AddressBook.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace AddressBook.Application.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly ICommonRepository<ApplicationUser> _userRepository;
        private readonly ICommonRepository<EmployeeAddressBook> _addressBookRepository;
        private readonly ICommonRepository<Department> _departmentRepository;
        private readonly ICommonRepository<Job> _jobRepository;

        public DashboardService(ICommonRepository<ApplicationUser> userRepository, 
            ICommonRepository<EmployeeAddressBook> addressBookRepository,
            ICommonRepository<Department> departmentRepository,
            ICommonRepository<Job> jobRepository)
        {
            _userRepository = userRepository;
            _addressBookRepository = addressBookRepository;
            _departmentRepository = departmentRepository;
            _jobRepository = jobRepository;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var dashboardData = new DashboardDto
            {
                UsersCount = await _userRepository.CountAsync(),
                EmployeeCount = await _addressBookRepository.CountAsync(),
                DepartmentCount = await _departmentRepository.CountAsync(),
                JobsCount = await _jobRepository.CountAsync(),
            };

            return dashboardData;
        }
    }
}
