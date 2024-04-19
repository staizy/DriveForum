using System.ComponentModel.DataAnnotations;

namespace DriveForum.ViewModels
{
    public class AuthUser
    {
        [Required(ErrorMessage = "Нельзя оставить пустым!")]
        [StringLength(maximumLength: 18, MinimumLength = 4, ErrorMessage = "Длина ника должна быть от 4 до 18 символов!")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Только латиница!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Нельзя оставить пустым!")]
        [EmailAddress(ErrorMessage = "Введите валидный адрес!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Нельзя оставить пустым!")]
        [StringLength(maximumLength: 18, MinimumLength = 4, ErrorMessage = "Длина логина должна быть от 4 до 18 символов!")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Только латиница!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Нельзя оставить пустым!")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+", ErrorMessage = "Только латица + минимум 1 цифра!")]
        [StringLength(maximumLength: 18, MinimumLength = 4, ErrorMessage = "Длина пароля должна быть от 4 до 18 символов!")]
        public string Password { get; set; }
    }
}
