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
    public class DeviceProfileWebService : ControllerBase
    {
        private readonly IDeviceProfileService _deviceProfileService;

        public DeviceProfileWebService(IDeviceProfileService deviceProfileService)
        {
            _deviceProfileService = deviceProfileService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceProfile(int id)
        {
            var response = new ApiResponse<DeviceProfile>();
            var deviceProfile = await _deviceProfileService.GetByIdAsync(id);

            if (deviceProfile == null)
            {
                response.Success = false;
                response.Message = "Device Profile not found.";
                return NotFound(response);
            }

            response.Success = true;
            response.Message = "Device Profile retrieved successfully.";
            response.Data = deviceProfile;
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeviceProfiles()
        {
            var response = new ApiResponse<IEnumerable<DeviceProfile>>();
            var deviceProfiles = await _deviceProfileService.GetAllAsync();

            response.Success = true;
            response.Message = "Device Profiles retrieved successfully.";
            response.Data = deviceProfiles;
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeviceProfile([FromBody] DeviceProfile deviceProfile)
        {
            var response = new ApiResponse<DeviceProfile>();

            var createdDeviceProfile = await _deviceProfileService.CreateAsync(deviceProfile);

            response.Success = true;
            response.Message = "Device Profile created successfully.";
            response.Data = createdDeviceProfile;

            return CreatedAtAction(nameof(GetDeviceProfile), new { id = createdDeviceProfile.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeviceProfile(int id, [FromBody] DeviceProfile deviceProfile)
        {
            var response = new ApiResponse<object>();

            if (id != deviceProfile.Id)
            {
                response.Success = false;
                response.Message = "Device Profile ID mismatch.";
                return BadRequest(response);
            }

            await _deviceProfileService.UpdateAsync(deviceProfile);
            response.Success = true;
            response.Message = "Device Profile updated successfully.";
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceProfile(int id)
        {
            var response = new ApiResponse<object>();

            await _deviceProfileService.DeleteAsync(id);
            response.Success = true;
            response.Message = "Device Profile deleted successfully.";
            return NoContent();
        }

        [HttpGet("page/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetDeviceProfilesByPage(int pageNumber, int pageSize)
        {
            var response = new ApiResponse<IEnumerable<DeviceProfile>>();
            var deviceProfiles = await _deviceProfileService.GetDeviceProfilesByPageAsync(pageNumber, pageSize);

            response.Success = true;
            response.Message = "Device Profiles retrieved successfully.";
            response.Data = deviceProfiles;
            return Ok(response);
        }
    }
}
