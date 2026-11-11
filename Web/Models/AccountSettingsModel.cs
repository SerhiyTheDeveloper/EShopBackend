using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class AccountSettingsModel
    {
        [Required(ErrorMessage = "Введіть ім'я")]
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть номер телефону")]
        [Phone(ErrorMessage = "Невірний формат номера")]
        [MaxLength(15, ErrorMessage = "Максимум 15 символів")]
        public string Phone { get; set; } = string.Empty;
    }
}
