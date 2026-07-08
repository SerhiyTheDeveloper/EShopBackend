using System.ComponentModel.DataAnnotations;

namespace Web.Models
{

    public class RegisterModel
    {
        [Required(ErrorMessage = "Введіть пошту")]
        [EmailAddress(ErrorMessage = "Невірний формат пошти")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль")]
        [MinLength(8, ErrorMessage = "Мінімум 8 символів")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть ім'я")]
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть номер телефону")]
        [Phone(ErrorMessage = "Невірний формат номера")]
        public string Phone { get; set; } = string.Empty;
    }
}
