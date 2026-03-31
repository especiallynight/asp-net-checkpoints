using System.ComponentModel.DataAnnotations;

namespace Checkpoint_9.Models
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Имя обязательно")]
        [MinLength(3, ErrorMessage = "Имя должно содержать не менее 3 символов")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я]+$", ErrorMessage = "Имя должно содержать только буквы")]
        [Display(Name = "Имя пользователя")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилия обязательна")]
        [MinLength(3, ErrorMessage = "Фамилия должна содержать не менее 3 символов")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я]+$", ErrorMessage = "Фамилия должна содержать только буквы")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email. Email должен содержать @ и .")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Пароль должен содержать не менее 8 символов")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$",
            ErrorMessage = "Пароль должен содержать и буквы, и цифры")]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;


        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
