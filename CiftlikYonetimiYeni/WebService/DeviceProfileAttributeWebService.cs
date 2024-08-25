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
    public class DeviceProfileAttributeWebService : ControllerBase
    {
        private readonly IDeviceProfileAttributeService _deviceProfileAttributeService;

        public DeviceProfileAttributeWebService(IDeviceProfileAttributeService deviceProfileAttributeService)
        {
            _deviceProfileAttributeService = deviceProfileAttributeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceProfileAttribute(int id)
        {
            var response = new ApiResponse<DeviceProfileAttribute>();
            var deviceProfileAttribute = await _deviceProfileAttributeService.GetByIdAsync(id);

            if (deviceProfileAttribute == null)
            {
                response.Success = false;
                response.Message = "Device Profile Attribute not found.";
                return NotFound(response);
            }

            response.Success = true;
            response.Message = "Device Profile Attribute retrieved successfully.";
            response.Data = deviceProfileAttribute;
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeviceProfileAttributes()
        {
            var response = new ApiResponse<IEnumerable<DeviceProfileAttribute>>();
            var deviceProfileAttributes = await _deviceProfileAttributeService.GetAllAsync();

            response.Success = true;
            response.Message = "Device Profile Attributes retrieved successfully.";
            response.Data = deviceProfileAttributes;
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeviceProfileAttribute([FromBody] DeviceProfileAttribute deviceProfileAttribute)
        {
            var response = new ApiResponse<DeviceProfileAttribute>();

            var createdDeviceProfileAttribute = await _deviceProfileAttributeService.CreateAsync(deviceProfileAttribute);

            response.Success = true;
            response.Message = "Device Profile Attribute created successfully.";
            response.Data = createdDeviceProfileAttribute;

            return CreatedAtAction(nameof(GetDeviceProfileAttribute), new { id = createdDeviceProfileAttribute.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeviceProfileAttribute(int id, [FromBody] DeviceProfileAttribute deviceProfileAttribute)
        {
            var response = new ApiResponse<object>();

            if (id != deviceProfileAttribute.Id)
            {
                response.Success = false;
                response.Message = "Device Profile Attribute ID mismatch.";
                return BadRequest(response);
            }

            await _deviceProfileAttributeService.UpdateAsync(deviceProfileAttribute);
            response.Success = true;
            response.Message = "Device Profile Attribute updated successfully.";
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceProfileAttribute(int id)
        {
            var response = new ApiResponse<object>();

            await _deviceProfileAttributeService.DeleteAsync(id);
            response.Success = true;
            response.Message = "Device Profile Attribute deleted successfully.";
            return NoContent();
        }

        [HttpGet("page/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetDeviceProfileAttributesByPage(int pageNumber, int pageSize)
        {
            var response = new ApiResponse<IEnumerable<DeviceProfileAttribute>>();
            var deviceProfileAttributes = await _deviceProfileAttributeService.GetDeviceProfileAttributesByPageAsync(pageNumber, pageSize);

            response.Success = true;
            response.Message = "Device Profile Attributes retrieved successfully.";
            response.Data = deviceProfileAttributes;
            return Ok(response);
        }
    }
}
