using Checkpoint_19.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Checkpoint_19.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
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

        public void OnGet()
        {
        }

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
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Input.Username),
                new Claim("CreatedAt", DateTime.UtcNow.ToString("O"))
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );

            return RedirectToPage("/Chat/ChatPage");
        }
    }
}