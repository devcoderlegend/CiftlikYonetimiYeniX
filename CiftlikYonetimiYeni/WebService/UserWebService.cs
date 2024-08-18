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

        public UserWebService(
            IUserService userService,
            IUserSessionService userSessionService,
            IOptions<JwtSettings> jwtSettings,
            IRefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _userSessionService = userSessionService;
            _jwtSettings = jwtSettings.Value;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.AuthenticateUserAsync(model.Email, model.Password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Kullanıcı cihazı Web ise (DeviceId -1 olarak gönderildiğinde)
            if (model.DeviceId == "-1")
            {
                // UserDevice kaydı yapılmaz
                var tokenString = GenerateJwtToken(user);

                // Refresh token oluşturma
                var refreshToken = new RefreshToken
                {
                    Token = GenerateRefreshToken(),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    UserId = user.Id
                };

                // Refresh token'ı veritabanına kaydet
                await _refreshTokenService.CreateAsync(refreshToken);

                return Ok(new
                {
                    Token = tokenString,
                    RefreshToken = refreshToken.Token
                });
            }

            // Diğer cihazlar için işlemler (DeviceId -1 değilse)
            // Burada isteğe bağlı olarak cihaz ile ilgili işlemler yapılabilir
            // Eğer ek bir işlem yapılması gerekiyorsa buraya ekleyebilirsiniz

            return Ok(new { Message = "Login successful but no device action was taken" });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        {
            var refreshToken = await _refreshTokenService.GetByTokenAsync(model.RefreshToken);
            if (refreshToken == null || !await _refreshTokenService.IsTokenActiveAsync(model.RefreshToken))
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            var user = await _userService.GetUserByIdAsync(refreshToken.UserId);

            // Yeni JWT token oluşturma
            var tokenString = GenerateJwtToken(user);

            return Ok(new
            {
                Token = tokenString,
                RefreshToken = refreshToken.Token // Aynı refresh token'ı dönebilir veya yenisini oluşturabilirsiniz.
            });
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenModel model)
        {
            var refreshToken = await _refreshTokenService.GetByTokenAsync(model.RefreshToken);
            if (refreshToken == null || !await _refreshTokenService.IsTokenActiveAsync(model.RefreshToken))
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            await _refreshTokenService.RevokeAsync(model.RefreshToken);
            return Ok(new { Message = "Token revoked successfully" });
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserCreationModel model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                // Diğer özellikleri de ekleyin
            };

            var createdUser = await _userService.CreateUserAsync(user, model.Password);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel model)
        {
            var user = await _userService.AuthenticateUserAsync(model.Email, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(new { Message = "Authentication successful", UserId = user.Id });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var result = await _userService.ChangePasswordAsync(model.UserId, model.OldPassword, model.NewPassword);
            if (!result)
            {
                return BadRequest("Unable to change password. Please check your old password.");
            }

            return Ok(new { Message = "Password changed successfully" });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
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
        public string BrandName { get; set; }
        public string Model { get; set; }
        public string UserAgent { get; set; }
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
