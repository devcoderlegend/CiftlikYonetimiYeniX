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
    public class DeviceValueReceiveWebService : ControllerBase
    {
        private readonly IDeviceValueReceiveService _deviceValueReceiveService;

        public DeviceValueReceiveWebService(IDeviceValueReceiveService deviceValueReceiveService)
        {
            _deviceValueReceiveService = deviceValueReceiveService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceValueReceive(int id)
        {
            var response = new ApiResponse<DeviceValueReceive>();
            var deviceValueReceive = await _deviceValueReceiveService.GetByIdAsync(id);

            if (deviceValueReceive == null)
            {
                response.Success = false;
                response.Message = "Device Value Receive not found.";
                return NotFound(response);
            }

            response.Success = true;
            response.Message = "Device Value Receive retrieved successfully.";
            response.Data = deviceValueReceive;
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeviceValueReceives()
        {
            var response = new ApiResponse<IEnumerable<DeviceValueReceive>>();
            var deviceValueReceives = await _deviceValueReceiveService.GetAllAsync();

            response.Success = true;
            response.Message = "Device Value Receives retrieved successfully.";
            response.Data = deviceValueReceives;
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeviceValueReceive([FromBody] DeviceValueReceive deviceValueReceive)
        {
            var response = new ApiResponse<DeviceValueReceive>();

            try
            {
                var createdDeviceValueReceive = await _deviceValueReceiveService.CreateAsync(deviceValueReceive);

                response.Success = true;
                response.Message = "Device Value Receive created successfully.";
                response.Data = createdDeviceValueReceive;

                return CreatedAtAction(nameof(GetDeviceValueReceive), new { id = createdDeviceValueReceive.Id }, response);
            }
            catch (ArgumentException ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeviceValueReceive(int id, [FromBody] DeviceValueReceive deviceValueReceive)
        {
            var response = new ApiResponse<object>();

            if (id != deviceValueReceive.Id)
            {
                response.Success = false;
                response.Message = "Device Value Receive ID mismatch.";
                return BadRequest(response);
            }

            await _deviceValueReceiveService.UpdateAsync(deviceValueReceive);
            response.Success = true;
            response.Message = "Device Value Receive updated successfully.";
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceValueReceive(int id)
        {
            var response = new ApiResponse<object>();

            await _deviceValueReceiveService.DeleteAsync(id);
            response.Success = true;
            response.Message = "Device Value Receive deleted successfully.";
            return NoContent();
        }

        [HttpGet("page/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetDeviceValueReceivesByPage(int pageNumber, int pageSize)
        {
            var response = new ApiResponse<IEnumerable<DeviceValueReceive>>();
            var deviceValueReceives = await _deviceValueReceiveService.GetDeviceValueReceivesByPageAsync(pageNumber, pageSize);

            response.Success = true;
            response.Message = "Device Value Receives retrieved successfully.";
            response.Data = deviceValueReceives;
            return Ok(response);
        }
    }
}
