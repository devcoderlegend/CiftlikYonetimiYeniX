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
    public class DeviceWebService : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceWebService(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice(int id)
        {
            var response = new ApiResponse<Device>();
            var device = await _deviceService.GetByIdAsync(id);

            if (device == null)
            {
                response.Success = false;
                response.Message = "Device not found.";
                return NotFound(response);
            }

            response.Success = true;
            response.Message = "Device retrieved successfully.";
            response.Data = device;
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDevices()
        {
            var response = new ApiResponse<IEnumerable<Device>>();
            var devices = await _deviceService.GetAllAsync();

            response.Success = true;
            response.Message = "Devices retrieved successfully.";
            response.Data = devices;
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDevice([FromBody] Device device)
        {
            var response = new ApiResponse<Device>();

            var createdDevice = await _deviceService.CreateAsync(device);

            response.Success = true;
            response.Message = "Device created successfully.";
            response.Data = createdDevice;

            return CreatedAtAction(nameof(GetDevice), new { id = createdDevice.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDevice(int id, [FromBody] Device device)
        {
            var response = new ApiResponse<object>();

            if (id != device.Id)
            {
                response.Success = false;
                response.Message = "Device ID mismatch.";
                return BadRequest(response);
            }

            await _deviceService.UpdateAsync(device);
            response.Success = true;
            response.Message = "Device updated successfully.";
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var response = new ApiResponse<object>();

            await _deviceService.DeleteAsync(id);
            response.Success = true;
            response.Message = "Device deleted successfully.";
            return NoContent();
        }

        [HttpGet("page/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetDevicesByPage(int pageNumber, int pageSize)
        {
            var response = new ApiResponse<IEnumerable<Device>>();
            var devices = await _deviceService.GetDevicesByPageAsync(pageNumber, pageSize);

            response.Success = true;
            response.Message = "Devices retrieved successfully.";
            response.Data = devices;
            return Ok(response);
        }
    }
}
