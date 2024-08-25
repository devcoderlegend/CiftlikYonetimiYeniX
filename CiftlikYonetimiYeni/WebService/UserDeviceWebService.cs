using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CiftlikYonetimiYeni.Models;

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

        // Kullanıcı cihazı kaydı yapma
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserDevice([FromBody] UserDevice userDevice)
        {
            var registeredUserDevice = await _userDeviceService.RegisterUserDeviceAsync(userDevice);
            return Ok(new
            {
                DeviceId = registeredUserDevice.Id,
                GeneratedKey = registeredUserDevice.GeneratedKey
            });
        }

        // Tüm kullanıcı cihazlarını listeleme
        [HttpGet]
        public async Task<IActionResult> GetAllUserDevices()
        {
            var userDevices = await _userDeviceService.GetAllUserDevicesAsync();
            return Ok(userDevices);
        }

        // Cihazı Id'ye göre getirme
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDeviceById(int id)
        {
            var userDevice = await _userDeviceService.GetUserDeviceByIdAsync(id);
            if (userDevice == null)
            {
                return NotFound("User device not found.");
            }
            return Ok(userDevice);
        }
    }
}
