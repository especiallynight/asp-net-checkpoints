using Checkpoint_19.Services;
using Checkpoint_20.Services;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint_19.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class RegisterRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!_userService.ValidateUser(request.Username, request.Password))
            {
                return Unauthorized(new { message = "Неверные учетные данные" });
            }

            var token = _tokenService.GenerateToken(request.Username);
            return Ok(new { token, username = request.Username });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (_userService.UserExists(request.Username))
            {
                return Conflict(new { message = "Пользователь уже существует" });
            }

            if (!_userService.Register(request.Username, request.Password))
            {
                return BadRequest(new { message = "Ошибка регистрации" });
            }

            var token = _tokenService.GenerateToken(request.Username);
            return Ok(new { token, username = request.Username });
        }
    }
}