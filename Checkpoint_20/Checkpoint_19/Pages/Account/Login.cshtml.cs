using Checkpoint_19.Services;
using Checkpoint_20.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Checkpoint_19.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public LoginModel(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
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

            var token = _tokenService.GenerateToken(Username);
            Response.Cookies.Append("jwt_token", token);

            return RedirectToPage("/Chat/ChatPage");
        }
    }
}