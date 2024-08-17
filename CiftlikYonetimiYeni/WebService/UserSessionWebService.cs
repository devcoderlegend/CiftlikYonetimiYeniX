//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using CiftlikYonetimiYeni.Services;

//namespace CiftlikYonetimiYeni.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class UserSessionController : ControllerBase
//    {
//        private readonly IUserSessionService _sessionService;
//        private readonly IUserService _userService;

//        public UserSessionController(IUserSessionService sessionService, IUserService userService)
//        {
//            _sessionService = sessionService;
//            _userService = userService;
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginModel model)
//        {
//            var user = await _userService.AuthenticateUserAsync(model.Email, model.Password);
//            if (user == null)
//            {
//                return Unauthorized();
//            }

//            var session = await _sessionService.CreateSessionAsync(user.Id, model.DeviceId, HttpContext.Connection.RemoteIpAddress.ToString());

//            return Ok(new
//            {
//                SessionKey = session.GeneratedKey,
//                ExpiresAt = session.ExpireTime
//            });
//        }

//        [HttpPost("logout")]
//        public async Task<IActionResult> Logout([FromBody] LogoutModel model)
//        {
//            await _sessionService.EndSessionAsync(model.SessionId);
//            return Ok();
//        }

//        [HttpGet("validate")]
//        public async Task<IActionResult> ValidateSession([FromQuery] int userId, [FromQuery] string sessionKey)
//        {
//            var isValid = await _sessionService.ValidateSessionAsync(userId, sessionKey);
//            return Ok(new { IsValid = isValid });
//        }
//    }

//    public class LoginModel
//    {
//        public string Email { get; set; }
//        public string Password { get; set; }
//        public int DeviceId { get; set; }
//    }

//    public class LogoutModel
//    {
//        public int SessionId { get; set; }
//    }
//}