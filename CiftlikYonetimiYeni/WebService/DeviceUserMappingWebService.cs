using Microsoft.AspNetCore.Mvc;
using CiftlikYonetimiYeni.Models;
using CiftlikYonetimiYeni.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CiftlikYonetimiYeni.WebService
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceUserMappingWebService : ControllerBase
    {
        private readonly IDeviceUserMappingService _deviceUserMappingService;

        public DeviceUserMappingWebService(IDeviceUserMappingService deviceUserMappingService)
        {
            _deviceUserMappingService = deviceUserMappingService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceUserMapping(int id)
        {
            var response = new ApiResponse<DeviceUserMapping>();
            var deviceUserMapping = await _deviceUserMappingService.GetByIdAsync(id);

            if (deviceUserMapping == null)
            {
                response.Success = false;
                response.Message = "Device-User Mapping not found.";
                return NotFound(response);
            }

            response.Success = true;
            response.Message = "Device-User Mapping retrieved successfully.";
            response.Data = deviceUserMapping;
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeviceUserMappings()
        {
            var response = new ApiResponse<IEnumerable<DeviceUserMapping>>();
            var deviceUserMappings = await _deviceUserMappingService.GetAllAsync();

            response.Success = true;
            response.Message = "Device-User Mappings retrieved successfully.";
            response.Data = deviceUserMappings;
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeviceUserMapping([FromBody] DeviceUserMapping deviceUserMapping)
        {
            var response = new ApiResponse<DeviceUserMapping>();

            var createdDeviceUserMapping = await _deviceUserMappingService.CreateAsync(deviceUserMapping);

            response.Success = true;
            response.Message = "Device-User Mapping created successfully.";
            response.Data = createdDeviceUserMapping;

            return CreatedAtAction(nameof(GetDeviceUserMapping), new { id = createdDeviceUserMapping.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeviceUserMapping(int id, [FromBody] DeviceUserMapping deviceUserMapping)
        {
            var response = new ApiResponse<object>();

            if (id != deviceUserMapping.Id)
            {
                response.Success = false;
                response.Message = "Device-User Mapping ID mismatch.";
                return BadRequest(response);
            }

            await _deviceUserMappingService.UpdateAsync(deviceUserMapping);
            response.Success = true;
            response.Message = "Device-User Mapping updated successfully.";
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceUserMapping(int id)
        {
            var response = new ApiResponse<object>();

            await _deviceUserMappingService.DeleteAsync(id);
            response.Success = true;
            response.Message = "Device-User Mapping deleted successfully.";
            return NoContent();
        }

        [HttpGet("page/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetDeviceUserMappingsByPage(int pageNumber, int pageSize)
        {
            var response = new ApiResponse<IEnumerable<DeviceUserMapping>>();
            var deviceUserMappings = await _deviceUserMappingService.GetDeviceUserMappingsByPageAsync(pageNumber, pageSize);

            response.Success = true;
            response.Message = "Device-User Mappings retrieved successfully.";
            response.Data = deviceUserMappings;
            return Ok(response);
        }
    }
}
