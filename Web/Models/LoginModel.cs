using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введіть пошту")]
        [EmailAddress(ErrorMessage = "Невірний формат пошти")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль")]
        [MinLength(8, ErrorMessage = "Мінімум 8 символів")]
        public string Password { get; set; } = string.Empty;
    }
}
