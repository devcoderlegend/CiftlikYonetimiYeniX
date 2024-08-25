using Microsoft.AspNetCore.Mvc;
using CiftlikYonetimiYeni.Models;
using CiftlikYonetimiYeni.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CiftlikYonetimiYeni.WebService
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentWebService : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentWebService(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var response = new ApiResponse<Department>();
            var department = await _departmentService.GetByIdAsync(id);

            if (department == null)
            {
                response.Success = false;
                response.Message = "Department not found.";
                return NotFound(response);
            }

            response.Success = true;
            response.Message = "Department retrieved successfully.";
            response.Data = department;
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var response = new ApiResponse<IEnumerable<Department>>();
            var departments = await _departmentService.GetAllAsync();

            response.Success = true;
            response.Message = "Departments retrieved successfully.";
            response.Data = departments;
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] Department department)
        {
            var response = new ApiResponse<Department>();

            var createdDepartment = await _departmentService.CreateAsync(department);

            response.Success = true;
            response.Message = "Department created successfully.";
            response.Data = createdDepartment;

            return CreatedAtAction(nameof(GetDepartment), new { id = createdDepartment.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department department)
        {
            var response = new ApiResponse<object>();

            if (id != department.Id)
            {
                response.Success = false;
                response.Message = "Department ID mismatch.";
                return BadRequest(response);
            }

            await _departmentService.UpdateAsync(department);
            response.Success = true;
            response.Message = "Department updated successfully.";
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var response = new ApiResponse<object>();

            await _departmentService.DeleteAsync(id);
            response.Success = true;
            response.Message = "Department deleted successfully.";
            return NoContent();
        }

        [HttpGet("page/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetDepartmentsByPage(int pageNumber, int pageSize)
        {
            var response = new ApiResponse<IEnumerable<Department>>();
            var departments = await _departmentService.GetDepartmentsByPageAsync(pageNumber, pageSize);

            response.Success = true;
            response.Message = "Departments retrieved successfully.";
            response.Data = departments;
            return Ok(response);
        }
    }

   
}
