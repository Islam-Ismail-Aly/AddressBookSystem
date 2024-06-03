using AddressBook.Application.DTOs.AddressBooks;
using AddressBook.Application.Utilities;
using AddressBook.Core.Entities;
using AddressBook.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace AddressBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "AddressBookAPIv1")]
    [SwaggerTag("Employee Address Book Management")]
    public class AddressBookController : ControllerBase
    {
        private readonly IUnitOfWork<EmployeeAddressBook> _unitOfWork;
        private readonly IMapper _mapper;

        public AddressBookController(IUnitOfWork<EmployeeAddressBook> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAllEmployees")]
        [SwaggerOperation(Summary = "Get all employees")]
        [SwaggerResponse(200, "List of employees", typeof(APIResponseResult<IEnumerable<EmployeeAddressBookDto>>))]
        [SwaggerResponse(404, "No employees found")]
        public async Task<ActionResult<APIResponseResult<IEnumerable<EmployeeAddressBookDto>>>> GetAllEmployees(DateTime? startDateOfBirth = null, DateTime? endDateOfBirth = null)
        {
            IQueryable<EmployeeAddressBook> query = _unitOfWork.Entity.GetAllQueryable();

            if (startDateOfBirth != null && endDateOfBirth != null)
            {
                query = query.Where(e => e.DateOfBirth >= startDateOfBirth && e.DateOfBirth <= endDateOfBirth);
            }

            var employees = await query.ToListAsync();

            if (employees == null || !employees.Any())
                return NotFound(new APIResponseResult<IEnumerable<EmployeeAddressBookDto>>("No employees found."));

            var employeeDtos = employees.Select(e => new EmployeeAddressBookDto
            {
                Id = e.Id,
                FullName = e.FullName,
                Address = e.Address,
                Age = e.Age,
                CreatedOn = e.CreatedOn,
                DateOfBirth = e.DateOfBirth,
                Email = e.Email,
                MobileNumber = e.MobileNumber,
                Photo = e.Photo,
                JobTitle = e.JobTitle?.Name,
                Department = e.Department?.Name
            });

            return Ok(new APIResponseResult<IEnumerable<EmployeeAddressBookDto>>(employeeDtos, "Employees retrieved successfully."));
        }


        [HttpGet("GetEmployeeById/{id:int}")]
        [SwaggerOperation(Summary = "Get an employee by ID")]
        [SwaggerResponse(200, "Employee details", typeof(APIResponseResult<EmployeeAddressBookDto>))]
        [SwaggerResponse(404, "Employee not found")]
        public async Task<ActionResult<APIResponseResult<EmployeeAddressBookDto>>> GetEmployeeById(int id)
        {
            var employee = await _unitOfWork.Entity.GetByIdAsync(id);
            
            if (employee == null)
                return NotFound(new APIResponseResult<EmployeeAddressBookDto>("Employee not found."));

            var employeeDto = _mapper.Map<EmployeeAddressBookDto>(employee);
            
            return Ok(new APIResponseResult<EmployeeAddressBookDto>(employeeDto, "Employee retrieved successfully."));
        }

        [HttpPost("AddEmployee")]
        [SwaggerOperation(Summary = "Add a new employee")]
        [SwaggerResponse(200, "Employee added successfully", typeof(APIResponseResult<EmployeeAddressBookDto>))]
        [SwaggerResponse(400, "Invalid employee data")]
        public async Task<ActionResult<APIResponseResult<EmployeeAddressBookDto>>> AddEmployee([FromBody] EmployeeAddressBookDto employeeDto)
        {
            if (employeeDto == null)
                return BadRequest(new APIResponseResult<EmployeeAddressBookDto>("Invalid employee data."));

            try
            {
                var employee = _mapper.Map<EmployeeAddressBook>(employeeDto);

                await _unitOfWork.Entity.InsertAsync(employee);
                await _unitOfWork.SaveAsync();

                var createdEmployeeDto = _mapper.Map<EmployeeAddressBookDto>(employee);
                return Ok(new APIResponseResult<EmployeeAddressBookDto>(createdEmployeeDto, "Employee added successfully."));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new APIResponseResult<EmployeeAddressBookDto>("An error occurred while saving the employee."));
            }
        }

        [HttpPut("UpdateEmployee/{id:int}")]
        [SwaggerOperation(Summary = "Update an employee")]
        [SwaggerResponse(200, "Employee updated successfully", typeof(APIResponseResult<EmployeeAddressBookDto>))]
        [SwaggerResponse(400, "Invalid employee data")]
        [SwaggerResponse(404, "Employee not found")]
        public async Task<ActionResult<APIResponseResult<EmployeeAddressBookDto>>> UpdateEmployee(int id, [FromBody] EmployeeAddressBookDto employeeDto)
        {
            if (employeeDto == null)
                return BadRequest(new APIResponseResult<EmployeeAddressBookDto>("Invalid employee data."));

            var employeeInDb = await _unitOfWork.Entity.GetByIdAsync(id);
            
            if (employeeInDb == null)
                return NotFound(new APIResponseResult<EmployeeAddressBookDto>("Employee not found."));

            _mapper.Map(employeeDto, employeeInDb);

            await _unitOfWork.Entity.UpdateAsync(employeeInDb);
            await _unitOfWork.SaveAsync();

            var updatedDto = _mapper.Map<EmployeeAddressBookDto>(employeeInDb);
            return Ok(new APIResponseResult<EmployeeAddressBookDto>(updatedDto, "Employee updated successfully."));
        }

        [HttpDelete("DeleteEmployee/{id:int}")]
        [SwaggerOperation(Summary = "Delete an employee")]
        [SwaggerResponse(200, "Employee deleted successfully")]
        [SwaggerResponse(404, "Employee not found")]
        public async Task<ActionResult<APIResponseResult<EmployeeAddressBookDto>>> DeleteEmployee(int id)
        {
            var employee = await _unitOfWork.Entity.GetByIdAsync(id);
            
            if (employee == null)
                return NotFound(new APIResponseResult<EmployeeAddressBookDto>("Employee not found."));

            await _unitOfWork.Entity.DeleteAsync(employee);
            await _unitOfWork.SaveAsync();

            return Ok(new APIResponseResult<EmployeeAddressBookDto>(null, "Employee deleted successfully."));
        }
    }
}
