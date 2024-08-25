using Microsoft.AspNetCore.Mvc;
using CiftlikYonetimiYeni.Models;
using CiftlikYonetimiYeni.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Helper;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System;

namespace CiftlikYonetimiYeni.WebService
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserWebService : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserSessionService _userSessionService;
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserDeviceService _userDeviceService;

        public UserWebService(
            IUserService userService,
            IUserSessionService userSessionService,
            IOptions<JwtSettings> jwtSettings,
            IRefreshTokenService refreshTokenService,
            IUserDeviceService userDeviceService)
        {
            _userService = userService;
            _userSessionService = userSessionService;
            _jwtSettings = jwtSettings.Value;
            _refreshTokenService = refreshTokenService;
            _userDeviceService = userDeviceService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var response = new ApiResponse<object>();

            // Handle desktop login (DeviceId = -1)
            if (model.DeviceId == "-1")
            {
                var user = await _userService.AuthenticateUserAsync(model.Email, model.Password);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid email or password.";
                    return Unauthorized(response);
                }

                var tokenString = GenerateJwtToken(user);
                var refreshToken = new RefreshToken
                {
                    Token = GenerateRefreshToken(),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    UserId = user.Id
                };

                await _refreshTokenService.CreateAsync(refreshToken);

                var devices = await _userService.GetUserDevicesAsync(user.Id); // Kullanıcının cihaz bilgilerini al

                response.Success = true;
                response.Message = "Login successful";
                response.Data = new
                {
                    Token = tokenString,
                    RefreshToken = refreshToken.Token,
                    Devices = devices  // Cihaz bilgilerini de yanıt olarak dön
                };

                return Ok(response);
            }

            // Handle mobile or other device login
            if (string.IsNullOrWhiteSpace(model.GeneratedKey))
            {
                response.Success = false;
                response.Message = "GeneratedKey is required for device login.";
                return BadRequest(response);
            }

            var userDevice = await _userDeviceService.GetOrCreateDeviceAsync(model.DeviceId, null, null, 2, null);
            if (userDevice == null || userDevice.GeneratedKey != model.GeneratedKey)
            {
                response.Success = false;
                response.Message = "Invalid device or device key.";
                return Unauthorized(response);
            }

            var userDeviceLogin = await _userService.AuthenticateUserAsync(model.Email, model.Password);
            if (userDeviceLogin == null)
            {
                response.Success = false;
                response.Message = "Invalid email or password.";
                return Unauthorized(response);
            }

            var deviceToken = GenerateJwtToken(userDeviceLogin);
            var newRefreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                UserId = userDeviceLogin.Id
            };

            await _refreshTokenService.CreateAsync(newRefreshToken);

            var devicesForUser = await _userService.GetUserDevicesAsync(userDeviceLogin.Id);  // Kullanıcının cihaz bilgilerini al

            response.Success = true;
            response.Message = "Login successful";
            response.Data = new
            {
                Token = deviceToken,
                RefreshToken = newRefreshToken.Token,
                UserDeviceId = userDevice.Id,
                GeneratedKey = userDevice.GeneratedKey,
                Devices = devicesForUser  // Cihaz bilgilerini de yanıt olarak dön
            };

            return Ok(response);
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        {
            var response = new ApiResponse<object>();

            var refreshToken = await _refreshTokenService.GetByTokenAsync(model.RefreshToken);
            if (refreshToken == null || !await _refreshTokenService.IsTokenActiveAsync(model.RefreshToken))
            {
                response.Success = false;
                response.Message = "Invalid or expired refresh token.";
                return Unauthorized(response);
            }

            var user = await _userService.GetUserByIdAsync(refreshToken.UserId);
            var tokenString = GenerateJwtToken(user);

            response.Success = true;
            response.Message = "Token refreshed successfully";
            response.Data = new
            {
                Token = tokenString,
                RefreshToken = refreshToken.Token
            };
            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenModel model)
        {
            var response = new ApiResponse<object>();

            var refreshToken = await _refreshTokenService.GetByTokenAsync(model.RefreshToken);
            if (refreshToken == null || !await _refreshTokenService.IsTokenActiveAsync(model.RefreshToken))
            {
                response.Success = false;
                response.Message = "Invalid or expired refresh token.";
                return Unauthorized(response);
            }

            await _refreshTokenService.RevokeAsync(model.RefreshToken);
            response.Success = true;
            response.Message = "Token revoked successfully";
            return Ok(response);
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = new ApiResponse<IEnumerable<User>>();
            var users = await _userService.GetAllUsersAsync();

            response.Success = true;
            response.Message = "Users retrieved successfully";
            response.Data = users;
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationModel model)
        {
            var response = new ApiResponse<User>();

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var createdUser = await _userService.CreateUserAsync(user, model.Password);

            response.Success = true;
            response.Message = "User created successfully";
            response.Data = createdUser;

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            var response = new ApiResponse<object>();

            if (id != user.Id)
            {
                response.Success = false;
                response.Message = "User ID mismatch.";
                return BadRequest(response);
            }

            await _userService.UpdateUserAsync(user);
            response.Success = true;
            response.Message = "User updated successfully";
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = new ApiResponse<object>();

            await _userService.DeleteUserAsync(id);
            response.Success = true;
            response.Message = "User deleted successfully";
            return NoContent();
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel model)
        {
            var response = new ApiResponse<object>();

            var user = await _userService.AuthenticateUserAsync(model.Email, model.Password);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Authentication failed.";
                return Unauthorized(response);
            }

            response.Success = true;
            response.Message = "Authentication successful";
            response.Data = new { UserId = user.Id };
            return Ok(response);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var response = new ApiResponse<object>();

            var result = await _userService.ChangePasswordAsync(model.UserId, model.OldPassword, model.NewPassword);
            if (!result)
            {
                response.Success = false;
                response.Message = "Unable to change password. Please check your old password.";
                return BadRequest(response);
            }

            response.Success = true;
            response.Message = "Password changed successfully";
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var response = new ApiResponse<User>();
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return NotFound(response);
            }

            response.Success = true;
            response.Message = "User retrieved successfully";
            response.Data = user;
            return Ok(response);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);  // Burada _jwtSettings.Secret null olabilir

            if (key == null || key.Length == 0)
            {
                throw new ArgumentNullException("JWT Secret Key is missing or invalid.");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }

    // Ortak Yanıt Modeli
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(bool success, string message, T data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }

    public class UserCreationModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DeviceId { get; set; }
        public string GeneratedKey { get; set; } // Optional when DeviceId is -1
    }

    public class RefreshTokenModel
    {
        public string RefreshToken { get; set; }
    }

    public class RevokeTokenModel
    {
        public string RefreshToken { get; set; }
    }

    public class ChangePasswordModel
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
