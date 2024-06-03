using AddressBook.Application.Utilities;
using AddressBook.Core.Entities;
using AddressBook.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using AddressBook.Application.DTOs.Jobs;

namespace AddressBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "JobAPIv1")]
    [SwaggerTag("Job Management")]
    public class JobController : ControllerBase
    {
        private readonly IUnitOfWork<Job> _unitOfWork;
        private readonly IMapper _mapper;

        public JobController(IUnitOfWork<Job> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAllJobs")]
        [SwaggerOperation(Summary = "Get all jobs")]
        [SwaggerResponse(200, "List of jobs", typeof(APIResponseResult<IEnumerable<JobDto>>))]
        [SwaggerResponse(404, "No jobs found")]
        public async Task<ActionResult<APIResponseResult<IEnumerable<JobDto>>>> GetAllJobs()
        {
            var jobs = await _unitOfWork.Entity.GetAllAsync();
            
            if (jobs == null)
                return NotFound(new APIResponseResult<IEnumerable<JobDto>>("No jobs found."));

            var jobDtos = _mapper.Map<IEnumerable<JobDto>>(jobs);
            
            return Ok(new APIResponseResult<IEnumerable<JobDto>>(jobDtos, "Jobs retrieved successfully."));
        }

        [HttpGet("GetJobById/{id:int}")]
        [SwaggerOperation(Summary = "Get a job by ID")]
        [SwaggerResponse(200, "Job details", typeof(APIResponseResult<JobDto>))]
        [SwaggerResponse(404, "Job not found")]
        public async Task<ActionResult<APIResponseResult<JobDto>>> GetJobById(int id)
        {
            var job = await _unitOfWork.Entity.GetByIdAsync(id);
           
            if (job == null)
                return NotFound(new APIResponseResult<JobDto>("Job not found."));

            var jobDto = _mapper.Map<JobDto>(job);
            
            return Ok(new APIResponseResult<JobDto>(jobDto, "Job retrieved successfully."));
        }

        [HttpPost("AddJob")]
        [SwaggerOperation(Summary = "Add a new job")]
        [SwaggerResponse(200, "Job added successfully", typeof(APIResponseResult<JobDto>))]
        [SwaggerResponse(400, "Invalid job data")]
        public async Task<ActionResult<APIResponseResult<JobDto>>> AddJob([FromBody] JobDto jobDto)
        {
            if (jobDto == null)
                return BadRequest(new APIResponseResult<JobDto>("Invalid job data."));

            try
            {
                var job = _mapper.Map<Job>(jobDto);

                await _unitOfWork.Entity.InsertAsync(job);
                await _unitOfWork.SaveAsync();

                var createdJobDto = _mapper.Map<JobDto>(job);
                return Ok(new APIResponseResult<JobDto>(createdJobDto, "Job added successfully."));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new APIResponseResult<JobDto>("An error occurred while saving the job."));
            }
        }

        [HttpPut("UpdateJob/{id:int}")]
        [SwaggerOperation(Summary = "Update a job")]
        [SwaggerResponse(200, "Job updated successfully", typeof(APIResponseResult<JobDto>))]
        [SwaggerResponse(400, "Invalid job data")]
        [SwaggerResponse(404, "Job not found")]
        public async Task<ActionResult<APIResponseResult<JobDto>>> UpdateJob(int id, [FromBody] JobDto jobDto)
        {
            if (jobDto == null)
                return BadRequest(new APIResponseResult<JobDto>("Invalid job data."));

            var jobInDb = await _unitOfWork.Entity.GetByIdAsync(id);
            if (jobInDb == null)
                return NotFound(new APIResponseResult<JobDto>("Job not found."));

            _mapper.Map(jobDto, jobInDb);

            await _unitOfWork.Entity.UpdateAsync(jobInDb);
            await _unitOfWork.SaveAsync();

            var updatedDto = _mapper.Map<JobDto>(jobInDb);
            return Ok(new APIResponseResult<JobDto>(updatedDto, "Job updated successfully."));
        }

        [HttpDelete("DeleteJob/{id:int}")]
        [SwaggerOperation(Summary = "Delete a job")]
        [SwaggerResponse(200, "Job deleted successfully")]
        [SwaggerResponse(404, "Job not found")]
        public async Task<ActionResult<APIResponseResult<JobDto>>> DeleteJob(int id)
        {
            var job = await _unitOfWork.Entity.GetByIdAsync(id);
            
            if (job == null)
                return NotFound(new APIResponseResult<JobDto>("Job not found."));

            await _unitOfWork.Entity.DeleteAsync(job);
            await _unitOfWork.SaveAsync();

            return Ok(new APIResponseResult<JobDto>(null, "Job deleted successfully."));
        }
    }
}
