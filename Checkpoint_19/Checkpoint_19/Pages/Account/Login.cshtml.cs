using Checkpoint_19.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Checkpoint_19.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            
            if (!_userService.UserExists(Username))
            {
                ModelState.AddModelError(string.Empty, "Пользователь не найден");
                return Page();
            }
            if (!_userService.ValidateUser(Username, Password))
            {
                ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль");
                return Page();
            }
            var user = _userService.GetUser(Username);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username),
                new Claim("CreatedAt", user?.CreatedAt.ToString("O") ?? DateTime.UtcNow.ToString("O"))
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