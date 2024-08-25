using Microsoft.AspNetCore.Mvc;
using CiftlikYonetimiYeni.Models;
using CiftlikYonetimiYeni.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CiftlikYonetimiYeni.WebService
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDeviceWebService : ControllerBase
    {
        private readonly IUserDeviceService _userDeviceService;

        public UserDeviceWebService(IUserDeviceService userDeviceService)
        {
            _userDeviceService = userDeviceService;
        }

        // Get all UserDevices
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDevice>>>> GetAllUserDevices()
        {
            var devices = await _userDeviceService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<UserDevice>>(true, "Devices retrieved successfully", devices));
        }

        // Get a UserDevice by ID
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDevice>>> GetUserDeviceById(int id)
        {
            var device = await _userDeviceService.GetByIdAsync(id);
            if (device == null)
            {
                return NotFound(new ApiResponse<UserDevice>(false, "Device not found"));
            }
            return Ok(new ApiResponse<UserDevice>(true, "Device retrieved successfully", device));
        }

        // Create a new UserDevice
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserDevice>>> CreateUserDevice([FromBody] UserDevice userDevice)
        {
            var createdDevice = await _userDeviceService.CreateAsync(userDevice);
            return CreatedAtAction(nameof(GetUserDeviceById), new { id = createdDevice.Id }, new ApiResponse<UserDevice>(true, "Device created successfully", createdDevice));
        }

        // Update a UserDevice
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UserDevice>>> UpdateUserDevice(int id, [FromBody] UserDevice userDevice)
        {
            if (id != userDevice.Id)
            {
                return BadRequest(new ApiResponse<UserDevice>(false, "Device ID mismatch"));
            }

            var existingDevice = await _userDeviceService.GetByIdAsync(id);
            if (existingDevice == null)
            {
                return NotFound(new ApiResponse<UserDevice>(false, "Device not found"));
            }

            await _userDeviceService.UpdateAsync(userDevice);
            return Ok(new ApiResponse<UserDevice>(true, "Device updated successfully"));
        }

        // Delete a UserDevice
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<UserDevice>>> DeleteUserDevice(int id)
        {
            var device = await _userDeviceService.GetByIdAsync(id);
            if (device == null)
            {
                return NotFound(new ApiResponse<UserDevice>(false, "Device not found"));
            }

            await _userDeviceService.DeleteAsync(id);
            return Ok(new ApiResponse<UserDevice>(true, "Device deleted successfully"));
        }

        // Get or create a UserDevice based on DeviceId
        [Authorize]
        [HttpPost("get-or-create")]
        public async Task<ActionResult<ApiResponse<UserDevice>>> GetOrCreateUserDevice([FromBody] UserDevice model)
        {
            var device = await _userDeviceService.GetOrCreateDeviceAsync(
                model.DeviceId,
                model.BrandName,
                model.Model,
                model.UserDeviceTypeId,
                model.UserAgent
            );

            return Ok(new ApiResponse<UserDevice>(true, "Device retrieved or created successfully", device));
        }
    }


}
