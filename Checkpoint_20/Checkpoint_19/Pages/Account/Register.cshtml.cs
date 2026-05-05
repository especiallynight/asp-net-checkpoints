using Checkpoint_19.Services;
using Checkpoint_20.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Checkpoint_19.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public RegisterModel(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Введите имя пользователя")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Введите пароль")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "Пароль должен быть не менее 6 символов")]
            public string Password { get; set; }
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (_userService.UserExists(Input.Username))
            {
                ModelState.AddModelError("Input.Username", "Пользователь с таким именем уже существует");
                return Page();
            }

            if (!_userService.Register(Input.Username, Input.Password))
            {
                ModelState.AddModelError(string.Empty, "Ошибка при регистрации. Попробуйте позже.");
                return Page();
            }

            var token = _tokenService.GenerateToken(Input.Username);

            Response.Cookies.Append("jwt_token", token, new CookieOptions
            {
                HttpOnly = false,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            TempData["JwtToken"] = token;
            TempData["Username"] = Input.Username;

            return RedirectToPage("/Chat/ChatPage");
        }
    }
}