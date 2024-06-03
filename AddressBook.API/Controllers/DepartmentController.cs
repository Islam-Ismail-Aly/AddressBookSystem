using AddressBook.Application.Utilities;
using AddressBook.Core.Entities;
using AddressBook.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using AddressBook.Application.DTOs.Departments;

namespace AddressBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "DepartmentAPIv1")]
    [SwaggerTag("Department Management")]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWork<Department> _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork<Department> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAllDepartments")]
        [SwaggerOperation(Summary = "Get all departments")]
        [SwaggerResponse(200, "List of departments", typeof(APIResponseResult<IEnumerable<DepartmentDto>>))]
        [SwaggerResponse(404, "No departments found")]
        public async Task<ActionResult<APIResponseResult<IEnumerable<DepartmentDto>>>> GetAllDepartments()
        {
            var departments = await _unitOfWork.Entity.GetAllAsync();
            
            if (departments == null)
                return NotFound(new APIResponseResult<IEnumerable<DepartmentDto>>("No departments found."));

            var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
            
            return Ok(new APIResponseResult<IEnumerable<DepartmentDto>>(departmentDtos, "Departments retrieved successfully."));
        }

        [HttpGet("GetDepartmentById/{id:int}")]
        [SwaggerOperation(Summary = "Get a department by ID")]
        [SwaggerResponse(200, "Department details", typeof(APIResponseResult<DepartmentDto>))]
        [SwaggerResponse(404, "Department not found")]
        public async Task<ActionResult<APIResponseResult<DepartmentDto>>> GetDepartmentById(int id)
        {
            var department = await _unitOfWork.Entity.GetByIdAsync(id);
           
            if (department == null)
                return NotFound(new APIResponseResult<DepartmentDto>("Department not found."));

            var departmentDto = _mapper.Map<DepartmentDto>(department);
            
            return Ok(new APIResponseResult<DepartmentDto>(departmentDto, "Department retrieved successfully."));
        }

        [HttpPost("AddDepartment")]
        [SwaggerOperation(Summary = "Add a new department")]
        [SwaggerResponse(200, "Department added successfully", typeof(APIResponseResult<DepartmentDto>))]
        [SwaggerResponse(400, "Invalid department data")]
        public async Task<ActionResult<APIResponseResult<DepartmentDto>>> AddDepartment([FromBody] DepartmentDto departmentDto)
        {
            if (departmentDto == null)
                return BadRequest(new APIResponseResult<DepartmentDto>("Invalid department data."));

            try
            {
                var department = _mapper.Map<Department>(departmentDto);

                await _unitOfWork.Entity.InsertAsync(department);
                await _unitOfWork.SaveAsync();

                var createdDepartmentDto = _mapper.Map<DepartmentDto>(department);
                return Ok(new APIResponseResult<DepartmentDto>(createdDepartmentDto, "Department added successfully."));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new APIResponseResult<DepartmentDto>("An error occurred while saving the department."));
            }
        }

        [HttpPut("UpdateDepartment/{id:int}")]
        [SwaggerOperation(Summary = "Update a department")]
        [SwaggerResponse(200, "Department updated successfully", typeof(APIResponseResult<DepartmentDto>))]
        [SwaggerResponse(400, "Invalid department data")]
        [SwaggerResponse(404, "Department not found")]
        public async Task<ActionResult<APIResponseResult<DepartmentDto>>> UpdateDepartment(int id, [FromBody] DepartmentDto departmentDto)
        {
            if (departmentDto == null)
                return BadRequest(new APIResponseResult<DepartmentDto>("Invalid department data."));

            var departmentInDb = await _unitOfWork.Entity.GetByIdAsync(id);
            
            if (departmentInDb == null)
                return NotFound(new APIResponseResult<DepartmentDto>("Department not found."));

            _mapper.Map(departmentDto, departmentInDb);

            await _unitOfWork.Entity.UpdateAsync(departmentInDb);
            await _unitOfWork.SaveAsync();

            var updatedDto = _mapper.Map<DepartmentDto>(departmentInDb);
            return Ok(new APIResponseResult<DepartmentDto>(updatedDto, "Department updated successfully."));
        }

        [HttpDelete("DeleteDepartment/{id:int}")]
        [SwaggerOperation(Summary = "Delete a department")]
        [SwaggerResponse(200, "Department deleted successfully")]
        [SwaggerResponse(404, "Department not found")]
        public async Task<ActionResult<APIResponseResult<DepartmentDto>>> DeleteDepartment(int id)
        {
            var department = await _unitOfWork.Entity.GetByIdAsync(id);
            
            if (department == null)
                return NotFound(new APIResponseResult<DepartmentDto>("Department not found."));

            _unitOfWork.Entity.DeleteAsync(department);
            await _unitOfWork.SaveAsync();

            return Ok(new APIResponseResult<DepartmentDto>(null, "Department deleted successfully."));
        }
    }
}
